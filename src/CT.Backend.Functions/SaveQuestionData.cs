using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CT.Backend.Shared;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;

namespace CT.Backend.Functions
{
    public static class SaveQuestionData
    {
        [FunctionName("SaveQuestionData")]                
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "QuestionsData",
                collectionName: "QuestionsData",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient outputTable,
            [Queue("NewQuestionsData")] IAsyncCollector<KeyValuePair<string, string>> outputQueue,
            ILogger log)
        {
            log.LogInformation("Try to add a new question.");
            QuestionData questionsToSave = null;
            try
            {
                
                questionsToSave = await System.Text.Json.JsonSerializer.DeserializeAsync<QuestionData>(req.Body,
                    new JsonSerializerOptions()
                    {
                        AllowTrailingCommas = true,

                    }
                );
            }
            catch (JsonException ex)
            {
                return new BadRequestObjectResult($"There was an error in the provided json: {ex.Message} -> {ex.InnerException.Message}");                
            }            
            questionsToSave.Source = "covapp.charite";
            await outputTable.CreateDocumentAsync("dbs/QuestionsData/colls/QuestionsData", questionsToSave);
            await outputQueue.AddAsync(questionsToSave.GetIdentifier());
            return new OkObjectResult(questionsToSave.Token);

        }        
    }
}
