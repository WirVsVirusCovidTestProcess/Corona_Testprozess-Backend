using CT.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace CT.Backend.Shared
{
    public static class SendGridConfigBuilder
    {
        public static SendGridConfig GetConfigFromEnvVars()
        {

            SendGridConfig sendGridConfig = new SendGridConfig();

            sendGridConfig.sendGridApiUser = Environment.GetEnvironmentVariable("SendGrid_ApiUser");
            sendGridConfig.sendGridApiKey = Environment.GetEnvironmentVariable("SendGrid_ApiKey");

            if (sendGridConfig.sendGridApiKey == null || sendGridConfig.sendGridApiUser == null)
            {
                throw new Exception("SendGrid configuration not found in environment vars or incomplete");
            }

            return sendGridConfig;
        }
    }
}
