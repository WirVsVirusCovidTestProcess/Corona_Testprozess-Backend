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

namespace CT.Backend.Functions.Testcenters
{
    public static class GetAllAssignedAppointments
    {
        [FunctionName("GetAllAssignedAppointments")]
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
                .Where(p => p.Assigend == true && p.TestResult == null);
            StringValues location;
            if (!req.Query.TryGetValue("location", out location))
            {
                log.LogInformation($"location porvided. try to get all assinged appointments from location.");
                requestQuery = requestQuery.Where(p => p.Location.ToString() == location);
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
            return new OkObjectResult(allResults.OrderBy(d => d.DateToBeInTestcenter));
        }
    }
}
