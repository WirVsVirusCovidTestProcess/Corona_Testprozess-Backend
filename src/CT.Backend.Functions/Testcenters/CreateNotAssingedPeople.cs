using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CT.Backend.Shared.Models;
using CT.Backend.Shared.ViewModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CT.Backend.Functions.Testcenters
{
    public static class CreateNotAssingedPeople
    {
        [FunctionName("CreateNotAssingedPeople")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "UserInformation",
            collectionName: "UserInformation",
            ConnectionStringSetting = "UserInformationDBConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> UserInformation,
            [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient notAssingedUsers,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient usersTable,
            ILogger log)
        {
            foreach (var modifiedUser in UserInformation.Where(d => d.GetPropertyValue<int?>("RiskScore") > 0 && string.IsNullOrWhiteSpace(d.GetPropertyValue<string>("AppointmentToken"))))
            {
                Uri userUri = UriFactory.CreateDocumentUri("UserInformation", "UserInformation", modifiedUser.Id);
                var fulluser = await usersTable.ReadDocumentAsync<Shared.Models.UserInformation>(userUri, new RequestOptions() { PartitionKey = new PartitionKey(modifiedUser.GetPropertyValue<string>("Source"))});
                log.LogInformation($"Create appointment for user: {fulluser.Document.Token}");
                var appointment = new Appointment();
                appointment.Assigend = false;
                appointment.RiskScore = fulluser.Document.RiskScore;
                appointment.Location = fulluser.Document.Location;
                appointment.TestResult = null;
                await notAssingedUsers.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers"), appointment);
                log.LogInformation($"Add appointment to user: {fulluser.Document.Token}");
                fulluser.Document.AppointmentToken = appointment.Token;
                await usersTable.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("UserInformation", "UserInformation", fulluser.Document.Id), fulluser.Document);
                log.LogInformation($"Finish adding empty appointment");
            }
        }
    }
}
