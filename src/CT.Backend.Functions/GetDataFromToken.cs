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

namespace CT.Backend.Functions
{
    public static class GetDataFromToken
    {
        [FunctionName("GetDataFromToken")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("QuestionsData")] CloudTable outputTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<TokenModel>(requestBody);
            if (string.IsNullOrEmpty(data.Token))
            {
                return new BadRequestObjectResult("You need a token to retrive data.");
            }

            var retriveOperation = TableOperation.Retrieve<QuestionData>("covapp.charite", data.Token);
            var result = await outputTable.ExecuteAsync(retriveOperation);
            var userData = result.Result as QuestionData;
            if (userData == null)
            {
                return new BadRequestObjectResult("The provided token was not valid!");
            }

            return new OkObjectResult(userData);
        }

        private class TokenModel
        {
            public string Token { get; set; }
        }
    }

}
