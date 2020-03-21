using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using CT.Backend.Shared.Models;
using System.Text.Json;
using System.Linq;
using Microsoft.Azure.Documents.Linq;

namespace CT.Backend.Functions.Testcenters
{
    public static class AddTestResultToAppointment
    {
        [FunctionName("AddTestResultToAppointment")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
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

                    }
                );
            }
            catch (System.Text.Json.JsonException ex)
            {
                return new BadRequestObjectResult($"There was an error in the provided json: {ex.Message} -> {ex.InnerException.Message}");
            }
            Uri questionsCollectionUri = UriFactory.CreateDocumentCollectionUri("Appointment", "AppointmentForUsers");
            var apointmentsQuery = appointments.CreateDocumentQuery<Appointment>(questionsCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Id == appointment.Id)
                .AsDocumentQuery<Appointment>();

            if (!apointmentsQuery.HasMoreResults)
            {
                return new BadRequestObjectResult($"The appointment Id {appointment.Id} doesn't exsists");
            }
            var result = await apointmentsQuery.ExecuteNextAsync<Appointment>();
            var appointmentResult = result.First();

            appointmentResult.TestResult = appointment.TestResult;
            await appointments.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("Appointment", "AppointmentForUsers", appointmentResult.Id), appointmentResult);
            return new OkObjectResult(appointmentResult);
        }
    }
}
