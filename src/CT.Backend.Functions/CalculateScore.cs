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
using Microsoft.WindowsAzure.Storage.Table;

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
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient outputTable,
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
            await outputTable.UpsertDocumentAsync(collectionUri, data, disableAutomaticIdGeneration: true);

            //var operation = TableOperation.Retrieve<QuestionData>(myQueueItem.Key, myQueueItem.Value);
            //var result = await outputTable.ExecuteAsync(operation);
            ////dynamic lol = result.Result;
            ////var anwers = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(lol.Answers);
            //var data = (result.Result as QuestionData);
            
            ////data.RiskScore = new ChariteCalculator().Calculate(data.Answers);
            //data.RiskScore = 20; // TODO: this deletes the answers...
            //log.LogInformation($"Update riskscore of {data.RowKey} to {data.RiskScore}");
            //var mergeOperation = TableOperation.Replace(data);
            //await outputTable.ExecuteAsync(mergeOperation);            
        }

        //private class QuestionRetriveData: TableEntity
        //{
        //    /// <summary>
        //    /// The answered questions
        //    /// </summary>
        //    public string Answers { get; set; }
        //    /// <summary>
        //    /// How high is the risk.
        //    /// </summary>
        //    public int? RiskScore { get; set; }

        //    public QuestionData ConvertToData()
        //    {
        //        return new QuestionData(RowKey)
        //        {
        //            PartitionKey = PartitionKey,
        //            Timestamp = Timestamp,
        //            ETag = ETag,
        //            RiskScore = RiskScore,
        //            Answers = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(Answers)
        //        };
        //    }
        //}
    }
}
