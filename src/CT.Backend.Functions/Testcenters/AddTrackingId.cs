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
using System.Text.Json;
using System.Linq;
using Microsoft.Azure.Documents.Linq;

namespace CT.Backend.Functions.Testcenters
{
    public static class AddTrackingId
    {
        [FunctionName("AddTrackingId")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
             [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient appointments,
            ILogger log)
        {
            Appointment appointment = null;
            try
            {
                appointment = await System.Text.Json.JsonSerializer.DeserializeAsync<Appointment>(req.Body,
                    new JsonSerializerOptions()
                    {
                        AllowTrailingCommas = true,
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (System.Text.Json.JsonException ex)
            {
                return new BadRequestObjectResult($"There was an error in the provided json: {ex.Message} -> {ex.InnerException.Message}");
            }
            Uri questionsCollectionUri = UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers");
            var appointmentsQuery = appointments.CreateDocumentQuery<Appointment>(questionsCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Token == appointment.Token)
                .AsDocumentQuery<Appointment>();

            if (!appointmentsQuery.HasMoreResults)
            {
                return new BadRequestObjectResult($"The appointment Id {appointment.Id} doesn't exsists");
            }
            var result = await appointmentsQuery.ExecuteNextAsync<Appointment>();
            var appointmentResult = result.First();

            appointmentResult.TrackingId = appointment.TrackingId;
            await appointments.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("Appointment", "AppointmentForUsers", appointmentResult.Id), appointmentResult);
            return new OkObjectResult(appointmentResult);
        }
    }
}
