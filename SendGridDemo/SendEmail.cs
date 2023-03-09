using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace SendGridDemo
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
        ILogger log, ExecutionContext context)
        {

            EmailMessage emailMessage = new EmailMessage();

            try
            {
                emailMessage = await req.Content.ReadAsAsync<EmailMessage>();
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            //Get SendGrid API key from application settings
            string apikey = Environment.GetEnvironmentVariable("SendGridApiKey");
            return await SendGridHelper.SendSimpleMailAsync(apikey, emailMessage);

        }
    }


}
