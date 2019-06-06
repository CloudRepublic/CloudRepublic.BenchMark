using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public static class Scheduler
    {
        [FunctionName("BenchMarkScheduler")]
        public static async Task RunScheduled([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("BenchMarkOrchestrator", null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}