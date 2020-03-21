using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CT.Backend.Shared;
using CT.Backend.Shared.ScoreCalculators;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CT.Backend.Functions
{
    public static class CalculateScore
    {
        [FunctionName("CalculateScore")]
        public static async Task Run(
            [QueueTrigger("NewQuestionsData")]KeyValuePair<string, string> myQueueItem,
            [CosmosDB(
                databaseName: "QuestionsData",
                collectionName: "QuestionsData",
                ConnectionStringSetting = "QuestionsDBConnection")] DocumentClient outputTable,
            ILogger log)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("QuestionsData", "QuestionsData");
            IDocumentQuery<QuestionData> query = outputTable.CreateDocumentQuery<QuestionData>(collectionUri)
                .Where(p => p.Source == myQueueItem.Key && p.Token == myQueueItem.Value)
                .AsDocumentQuery<QuestionData>();
            if (!query.HasMoreResults)
            {
                throw new ArgumentException($"The token wasn't found: {myQueueItem.Value}");
            }
            var result = await query.ExecuteNextAsync<QuestionData>();
            var data = result.First();
            log.LogInformation($"Start calculation of riskscore of {data.Token}");
            data.RiskScore = new ChariteCalculator().Calculate(data.Answers);
            log.LogInformation($"Update riskscore of {data.Token} to {data.RiskScore}");
            await outputTable.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("QuestionsData", "QuestionsData", data.Id), data );      
        }
    }
}
