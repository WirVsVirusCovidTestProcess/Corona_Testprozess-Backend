using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CT.Backend.Shared;
using CT.Backend.Shared.Models;
using CT.Backend.Shared.ScoreCalculators;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace CT.Backend.Functions
{
    public static class CalculateScore
    {
        [FunctionName("CalculateScore")]
        public static async Task Run(
            [QueueTrigger("NewQuestionsData")]KeyValuePair<string, string> myQueueItem,
            [Table("QuestionsData")] CloudTable outputTable,
            ILogger log)
        {
            var operation = TableOperation.Retrieve<QuestionRetriveData>(myQueueItem.Key, myQueueItem.Value);
            var result = await outputTable.ExecuteAsync(operation);
            dynamic lol = result.Result;
            var anwers = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(lol.Answers);
            var data = (result.Result as QuestionRetriveData).ConvertToData();
            log.LogInformation($"Start calculation of riskscore of {data.RowKey}");
            data.RiskScore = new ChariteCalculator().Calculate(data.Answers);
            log.LogInformation($"Update riskscore of {data.RowKey} to {data.RiskScore}");
            var mergeOperation = TableOperation.Replace(data);
            await outputTable.ExecuteAsync(mergeOperation);            
        }

        private class QuestionRetriveData: TableEntity
        {
            /// <summary>
            /// The answered questions
            /// </summary>
            public string Answers { get; set; }
            /// <summary>
            /// How high is the risk.
            /// </summary>
            public int? RiskScore { get; set; }

            public QuestionData ConvertToData()
            {
                return new QuestionData(RowKey)
                {
                    PartitionKey = PartitionKey,
                    Timestamp = Timestamp,
                    ETag = ETag,
                    RiskScore = RiskScore,
                    Answers = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(Answers)
                };
            }
        }
    }
}
