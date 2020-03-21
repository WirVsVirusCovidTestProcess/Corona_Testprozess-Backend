using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CT.Backend.Shared;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace CT.Backend.Functions
{
    public static class GetDataFromToken
    {
        [FunctionName("GetDataFromToken")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "QuestionsData",
                collectionName: "QuestionsData",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient outputTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<TokenModel>(requestBody);
            if (string.IsNullOrEmpty(data.Token))
            {
                return new BadRequestObjectResult("You need a token to retrive data.");
            }
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("QuestionsData", "QuestionsData");
            IDocumentQuery<QuestionData> query = outputTable.CreateDocumentQuery<QuestionData>(collectionUri)
                .Where(p => p.Source == "covapp.charite" && p.Token == data.Token)
                .AsDocumentQuery();
            if (!query.HasMoreResults)
            {
                return new BadRequestObjectResult($"The token wasn't found: {data.Token}");
            }
            var result = await query.ExecuteNextAsync();
            var userData = result.First() as QuestionData;
            return new OkObjectResult(userData);
        }

        private class TokenModel
        {
            public string Token { get; set; }
        }
    }

}
