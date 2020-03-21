using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CT.Backend.Shared
{
    public class QuestionData: TableEntity
    {
        public QuestionData() : this(GenerateToken()) { }
        public QuestionData(string RowKey)
        {
            this.RowKey = RowKey;
        }
        /// <summary>
        /// The source system of the data
        /// </summary>
        public new string PartitionKey { get { return base.PartitionKey; } set { base.PartitionKey = value; } }
        /// <summary>
        /// The token of the question data
        /// </summary>
        public new string RowKey { get { return base.RowKey; } set { base.RowKey = value; } }
        /// <summary>
        /// The answered questions
        /// </summary>
        public IEnumerable<Dictionary<string, string>> Answers { get; set; }
        /// <summary>
        /// How high is the risk.
        /// </summary>
        public int? RiskScore { get; set; }

        public KeyValuePair<string, string> GetIdentifier()
        {
            return new KeyValuePair<string, string>(PartitionKey, RowKey);
        }

        private static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token
                .Replace("/", ""); // table storage don't like '/'
        }
    }
}