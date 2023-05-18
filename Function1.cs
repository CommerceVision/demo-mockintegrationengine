using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MockIntegrationEngine
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation("Saying hello to {name}.", name);
            return $"Hello {name}!";
        }

        static string Version => "1.0.0";
        static List<string> Features => new List<string>
        {
            "Feature 01",
            "Feature 02",
            "Feature 03",
            "Feature 04",
            "Feature 05",
            "Feature 06",
            "Feature 10",
            "Feature 11",
        };

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(string.Join(", ", Features), Encoding.UTF8, "application/json")
            };
        }
    }
}