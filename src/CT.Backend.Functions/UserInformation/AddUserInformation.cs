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
using System.Text.Json;
using Microsoft.Azure.Documents;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CT.Backend.Functions.UserInformation
{
    public static class AddUserInformation
    {
        [FunctionName("AddUserInformation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient outputTable,
            [Queue("NewUserInformations")] IAsyncCollector<KeyValuePair<string, string>> outputQueue,
            ILogger log)
        {
            log.LogInformation("Try to add a new question.");
            Shared.Models.UserInformation userToSave = null;
            try
            {
                userToSave = await System.Text.Json.JsonSerializer.DeserializeAsync<Shared.Models.UserInformation>(req.Body,
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
            if(userToSave.RiskScore != null)
            {
                userToSave.RiskScore = null;
            }
            userToSave.Source = "covapp.charite";
            await outputTable.CreateDatabaseIfNotExistsAsync(new Database() { Id = "UserInformation" });
            await outputTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("UserInformation"), new DocumentCollection()
            {
                Id = "UserInformation",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/Source" }
                },
                UniqueKeyPolicy = new UniqueKeyPolicy()
                {
                    UniqueKeys = new Collection<UniqueKey>() {
                        new UniqueKey() {
                            Paths = new Collection<string>() { "/Token"}
                        }
                    }
                }
            });
            await outputTable.CreateDocumentAsync("dbs/UserInformation/colls/UserInformation", userToSave);
            await outputQueue.AddAsync(userToSave.GetIdentifier());
            return new OkObjectResult(userToSave.Token);
        }
    }
}
