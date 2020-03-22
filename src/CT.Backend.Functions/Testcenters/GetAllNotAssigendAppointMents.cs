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
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace CT.Backend.Functions.Testcenters
{
    public static class GetAllNotAssigendAppointMents
    {
        [FunctionName("GetAllNotAssigendAppointMents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient appointments,
            ILogger log)
        {
            
            Uri questionsCollectionUri = UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers");
            var requestQuery = appointments.CreateDocumentQuery<Appointment>(questionsCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Assigend != true && p.TestResult == null);
            StringValues locations;
            if (req.Query.TryGetValue("location", out locations))
            {
                log.LogInformation($"locations provided. try to get all not assinged appointments.");
                requestQuery = requestQuery.Where(p => locations.Contains(p.Location));
            }

            IDocumentQuery<Appointment> apointmentsQuery = requestQuery
                .AsDocumentQuery<Appointment>();
            if (!apointmentsQuery.HasMoreResults)
            {
                log.LogInformation("We havn't new infected people.");
                return new NoContentResult();
            }
            var allResults = new List<Appointment>();
            while (apointmentsQuery.HasMoreResults)
            {
                var apppointmentResult = await apointmentsQuery.ExecuteNextAsync<Appointment>();
                allResults.AddRange(apppointmentResult);
            }
            log.LogInformation($"Try to return {allResults.Count} appointments");
            return new OkObjectResult(allResults.OrderByDescending(d => d.RiskScore));
        }
    }
}
