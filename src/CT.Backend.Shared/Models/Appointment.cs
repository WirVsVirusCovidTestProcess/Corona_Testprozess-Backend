﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Backend.Shared.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Token = TokenGenerator.GenerateToken();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Token { get; set; }
        public bool Assigend { get; set; }
        public DateTimeOffset? DateToBeInTestcenter { get; set; }
        public string TestcenterAddress { get; set; }
        public int? RiskScore { get; set; }
        public string Location { get; set; }
        public string TrackingId { get; set; }        
        public bool? TestResult { get; set; }
    }
}
