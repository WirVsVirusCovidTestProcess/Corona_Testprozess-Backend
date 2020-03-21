using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CT.Backend.Shared.Models
{
    public class UserInformation
    {
        public UserInformation()
        {
            Token = TokenGenerator.GenerateToken();
        }
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Token { get; set; }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
        public int? RiskScore { get; set; }
        public int Location { get; set; }
        public string AppointmentToken { get; set; }
        public KeyValuePair<string, string> GetIdentifier()
        {
            return new KeyValuePair<string, string>(Source, Token);
        }

    }
}
