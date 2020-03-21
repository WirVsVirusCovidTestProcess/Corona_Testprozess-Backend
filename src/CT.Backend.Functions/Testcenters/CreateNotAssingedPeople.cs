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
            LeaseCollectionName = "leases")]IReadOnlyList<UserInformationViewModel> UserInformation,
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
            foreach (var modifiedUser in UserInformation.Where(d => d.RiskScore > 50 && string.IsNullOrWhiteSpace(d.AppointmentToken)))
            {
                log.LogInformation($"Create appointment for user: {modifiedUser.Token}");
                var appointment = new Appointment();
                appointment.Assigend = false;
                appointment.RiskScore = modifiedUser.RiskScore;
                appointment.Location = modifiedUser.Location;
                appointment.TestResult = null;
                await notAssingedUsers.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers"), appointment);
                log.LogInformation($"Add appointment to user: {modifiedUser.Token}");
                modifiedUser.AppointmentToken = appointment.Token;
                await usersTable.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("UserInformation", "UserInformation", modifiedUser.Id), modifiedUser);
                log.LogInformation($"Finish adding empty appointment");
            }
        }
    }
}
