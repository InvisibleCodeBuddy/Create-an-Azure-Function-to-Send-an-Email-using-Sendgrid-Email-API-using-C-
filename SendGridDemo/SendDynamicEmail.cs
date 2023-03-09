using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace SendGridDemo
{
    public static class SendDynamicEmail
    {
        [FunctionName("SendDynamicEmail")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
        ILogger log, ExecutionContext context)
        {
            DynamicEmailMessage emailMessage = new DynamicEmailMessage();
            //Get SendGrid API key from application settings
           
            try
            {
                emailMessage = await req.Content.ReadAsAsync<DynamicEmailMessage>();

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
            string apikey = Environment.GetEnvironmentVariable("SendGridApiKey");
            return await SendGridHelper.SendCustomMailAsync(apikey, emailMessage);
        }
    }
}
