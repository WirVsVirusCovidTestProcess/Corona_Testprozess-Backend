using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using CT.Backend.Shared.Models;
using CT.Backend.Shared;
using CT.Backend.Shared.MailSender;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace CT.Backend.Functions.UserInformation
{
    public static class NotifyTheUserAboutAnAppointment
    {
        [FunctionName("NotifyTheUserAboutAnAppointment")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "Appointment",
            collectionName: "AppointmentForUsers",
            ConnectionStringSetting = "AppointmentDBConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient usersTable,
            [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient appointmentTable,
            ILogger log)
        {
            Uri userCollectionUri = UriFactory.CreateDocumentCollectionUri("UserInformation", "UserInformation");
            // Notify the user about changed appointment
            foreach (var testResult in input.Where(d => d.GetPropertyValue<DateTimeOffset>("DateToBeInTestcenter") != null && d.GetPropertyValue<DateTimeOffset>("DateToBeInTestcenter") != DateTimeOffset.MinValue && d.GetPropertyValue<bool?>("TestResult") == null))
            {
                Uri appointmentUri = UriFactory.CreateDocumentUri("Appointment", "AppointmentForUsers", testResult.Id);
                var fullappointment = (await appointmentTable.ReadDocumentAsync<Appointment>(appointmentUri, new RequestOptions() {PartitionKey = new PartitionKey(testResult.Id) })).Document;
                IDocumentQuery<Shared.Models.UserInformation> userQuery = usersTable.CreateDocumentQuery<Shared.Models.UserInformation>(userCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                    .Where(p => p.AppointmentToken == fullappointment.Token)
                    .AsDocumentQuery<Shared.Models.UserInformation>();
                var result = await userQuery.ExecuteNextAsync<Shared.Models.UserInformation>();
                var userToInform = result.First();
                log.LogInformation($"Here we send mails to {userToInform.Email}"); // TODO: replace this with the sendgrid integration
            }

            // Notify the user about the test result
            foreach (var testResult in input.Where(d => d.GetPropertyValue<bool?>("TestResult") != null))
            {
                Uri appointmentUri = UriFactory.CreateDocumentUri("Appointment", "AppointmentForUsers", testResult.Id);
                var fullappointment = await appointmentTable.ReadDocumentAsync<Appointment>(appointmentUri, new RequestOptions() { PartitionKey = new PartitionKey(testResult.Id) });
                IDocumentQuery<Shared.Models.UserInformation> userQuery = usersTable.CreateDocumentQuery<Shared.Models.UserInformation>(userCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                    .Where(p => p.AppointmentToken == fullappointment.Document.Token)
                    .AsDocumentQuery<Shared.Models.UserInformation>();
                var result = await userQuery.ExecuteNextAsync<Shared.Models.UserInformation>();
                var userToInform = result.First();

                //Mail sending process
                var sendGridSender = new SendGridSender();
                var response = await sendGridSender
                    .SendMail("Corona_Testprozess@outlook.com", new List<string>() { userToInform.Email }, 
                    "CoronaTestPlattform - Your CoViD19 test appointment has been scheduled", 
                    $"Hi {userToInform.FirstName} </ br> your test was scheduled at the testcenter: <b> {userToInform.Location} </b>.", 
                    $"Hi {userToInform.FirstName} your test was scheduled at the testcenter: {userToInform.Location}.", 
                    SendGridConfigBuilder.GetConfigFromEnvVars().sendGridApiKey);
                if (response != HttpStatusCode.Accepted)
                {
                    log.LogError($"Error while trying to send mail to user. Got response code {response} from sendmail");
                }
                else
                {
                    log.LogInformation($"We have send an appointment mail to {userToInform.Email}");
                }

            }
        }
    }
}
