using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CT.Backend.Shared.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CT.Backend.Functions.UserInformation
{
    public static class NotifyUserAboutTestResult
    {
        [FunctionName("NotifyUserAboutTestResult")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "Appointment",
            collectionName: "AppointmentForUsers",
            ConnectionStringSetting = "AppointmentDBConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Appointment> input,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient usersTable,
            ILogger log)
        {
            Uri userCollectionUri = UriFactory.CreateDocumentCollectionUri("UserInformation", "UserInformation");
            foreach (var testResult in input.Where(d => d.TestResult != null))
            {
                IDocumentQuery<Shared.Models.UserInformation> userQuery = usersTable.CreateDocumentQuery<Shared.Models.UserInformation>(userCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                    .Where(p => p.AppointmentToken == testResult.Token)
                    .AsDocumentQuery<Shared.Models.UserInformation>();
                var result = await userQuery.ExecuteNextAsync<Shared.Models.UserInformation>();
                var userToInform = result.First();
                log.LogInformation($"Here we send mails to {userToInform.Email}"); // TODO: replace this with the sendgrid integration
            }
        }
    }
}
