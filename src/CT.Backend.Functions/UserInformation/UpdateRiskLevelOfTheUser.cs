using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using System.Text.Json;
using Microsoft.Azure.Documents.Linq;
using CT.Backend.Shared;
using System.Linq;
using CT.Backend.Shared.ViewModel;

namespace CT.Backend.Functions.UserInformation
{
    public static class UpdateRiskLevelOfTheUser
    {
        [FunctionName("UpdateRiskLevelOfTheUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "QuestionsData",
                collectionName: "QuestionsData",
                ConnectionStringSetting = "QuestionsDBConnection")] DocumentClient questionsTable,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient usersTable,
            ILogger log)
        {
            log.LogInformation("Start updating riskinformation.");
            Tokens tokens = null;
            try
            {
                tokens = await System.Text.Json.JsonSerializer.DeserializeAsync<Tokens>(req.Body,
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
            Uri questionsCollectionUri = UriFactory.CreateDocumentCollectionUri("QuestionsData", "QuestionsData");
            Uri userCollectionUri = UriFactory.CreateDocumentCollectionUri("UserInformation", "UserInformation");
            log.LogInformation("Try to get question and user information.");
            IDocumentQuery<QuestionData> questionsQuery = questionsTable.CreateDocumentQuery<QuestionData>(questionsCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Token == tokens.QuestionToken)
                .AsDocumentQuery<QuestionData>();
            IDocumentQuery<Shared.Models.UserInformation> userQuery = usersTable.CreateDocumentQuery<Shared.Models.UserInformation>(userCollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true })
                .Where(p => p.Token == tokens.UserToken)
                .AsDocumentQuery<Shared.Models.UserInformation>();
            if (!questionsQuery.HasMoreResults)
            {
                return new BadRequestObjectResult($"Your provided question token is not valid: {tokens.QuestionToken}");                
            }

            if (!userQuery.HasMoreResults)
            {
                return new BadRequestObjectResult($"Your provided user token is not valid: {tokens.UserToken}");
            }

            var questionResult = await questionsQuery.ExecuteNextAsync<QuestionData>();
            var questionData = questionResult.First();

            var userResult = await userQuery.ExecuteNextAsync<Shared.Models.UserInformation>();
            var userData = userResult.First();
            log.LogInformation($"Update risk level of user {tokens.UserToken}");
            userData.RiskScore = questionData.RiskScore;
            await usersTable.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("UserInformation", "UserInformation", userData.Id), userData);
            log.LogInformation($"Successful updated risk level of the user {tokens.UserToken}");
            return new OkObjectResult(new UserInformationViewModel(userData));
        }

        private class Tokens
        {
            public string UserToken { get; set; }
            public string QuestionToken { get; set; }
        }
    }
}
