using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace SendGridDemo
{
    public class SendGridHelper
    {


        public static async Task<HttpResponseMessage> SendSimpleMailAsync( string apiKey, EmailMessage emailMessage)
        {
            // Create a SendGrid message.
            SendGridMessage sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom(new EmailAddress(emailMessage.From));
            sendGridMessage.AddTo(new EmailAddress(emailMessage.To));
            sendGridMessage.SetSubject(emailMessage.Subject);
            sendGridMessage.AddContent(emailMessage.IsHTML ? MimeType.Html : MimeType.Text, emailMessage.Body);

            // Use the SendGrid API to send the email.
            var sendGridClient = new SendGridClient(apiKey);
            var response = await sendGridClient.SendEmailAsync(sendGridMessage);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        public static async Task<HttpResponseMessage> SendCustomMailAsync( string apiKey, DynamicEmailMessage emailMessage)
        {
            // Create a SendGrid message.
            SendGridMessage sendGridMessage = MailHelper.CreateSingleTemplateEmail(new EmailAddress(emailMessage.From), new EmailAddress(emailMessage.To), emailMessage.TemplateID , emailMessage.Body);


            // Use the SendGrid API to send the email.
            var sendGridClient = new SendGridClient(apiKey);
            var response = await sendGridClient.SendEmailAsync(sendGridMessage);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }

    //Simple Email Payload
    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHTML { get; set; }
    }

    //Dynamic Email Payload
    public class DynamicEmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string TemplateID { get; set; }
        public dynamic Body { get; set; }


    }
}
