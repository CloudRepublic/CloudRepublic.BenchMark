using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.API
{
    public static class WarmupTrigger
    {
        [FunctionName("WarmupTrigger")]
        public static async Task RunAsync([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
        {
            // warmup function
        }
    }
}