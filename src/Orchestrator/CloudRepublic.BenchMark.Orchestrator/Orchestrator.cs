using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public static class Orchestrator
    {
        [FunctionName("BenchMarkOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context,ILogger log)
        {
            var tasksWindows = new List<Task<long>>();

            //Call windows functions
            for (int i = 0; i < 10; i++)
            {
                tasksWindows.Add(context.CallActivityAsync<long>("BenchmarkRunner", null));
            }

            await Task.WhenAll(tasksWindows);

            var resultsWindows = tasksWindows.Select(t => t.Result);
            //TODO: write results to database

            var tasksLinux = new List<Task<long>>();
            //Call linux functions
            for (int i = 0; i < 10; i++)
            {
                tasksLinux.Add(context.CallActivityAsync<long>("BenchmarkRunner", null));
            }

            await Task.WhenAll(tasksLinux);

            var resultsLinux = tasksWindows.Select(t => t.Result);
            //TODO: write results to database
        }
    }
}