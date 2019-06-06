using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public static class Scheduler
    {
        [FunctionName("BenchMarkScheduler")]
        public static async Task RunScheduled(
#if DEBUG
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
#else
            [TimerTrigger("0 0 */4 * * *")] TimerInfo myTimer,
#endif
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("BenchMarkOrchestrator", null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}