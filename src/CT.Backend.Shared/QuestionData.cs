﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CT.Backend.Shared
{
    public class QuestionData
    {
        public QuestionData() : this(TokenGenerator.GenerateToken()) { }
        public QuestionData(string RowKey)
        {
            this.Token = RowKey;
        }
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// The source system of the data
        /// </summary>
        public  string Source { get; set; }
        /// <summary>
        /// The token of the question data
        /// </summary>
        public string Token { get; set; }
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
            return new KeyValuePair<string, string>(Source, Token);
        }
    }
}