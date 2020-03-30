using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using CT.Backend.Shared.Models;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Text;

namespace CT.Backend.Functions.Testcenters
{
    public static class GetAppointmentToUser
    {
        [FunctionName("GetAppointmentToUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient appointments,
            ILogger log)
        {
            var token = new StreamReader(req.Body, Encoding.UTF8).ReadToEnd();

            Uri questionsCollectionUri = UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers");
            var requestQuery = appointments.CreateDocumentQuery<Appointment>(questionsCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Token == token);
            if (requestQuery.Count() == 0)
            {
                return new NotFoundObjectResult("you don't have an appointment yet.");
            }
            return new OkObjectResult(requestQuery.First());
        }
    }
}
