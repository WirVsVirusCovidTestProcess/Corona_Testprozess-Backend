using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CT.Backend.Shared.MailSender
{
    public class SendGridSender
    {
        public async Task<HttpStatusCode> SendMail(string sendFromAddress, List<string> recipientsAddresses, string subject, string bodyText, string bodyHtml, string apiKey)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(sendFromAddress));

            var recipients = new List<EmailAddress>();

            foreach (var recipientAddress in recipientsAddresses)
            {
                recipients.Add(new EmailAddress(recipientAddress));
            }
            msg.AddTos(recipients);

            msg.SetSubject(subject);

            msg.AddContent(MimeType.Text, bodyText);
            msg.AddContent(MimeType.Html, bodyHtml);

            var client = new SendGridClient(apiKey);
            return client.SendEmailAsync(msg).Result.StatusCode;
        }
    }
}
