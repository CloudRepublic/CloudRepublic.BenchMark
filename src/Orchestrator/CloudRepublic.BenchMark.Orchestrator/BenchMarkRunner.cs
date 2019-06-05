using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class BenchMarkRunner
    {
        [FunctionName("BenchmarkRunner")]
        public static async Task<long> Benchmark([ActivityTrigger]  ILogger log)
        {
            var stopWatch = Stopwatch.StartNew();
            //TODO: make http call to function
            await Task.Delay(TimeSpan.FromSeconds(5));

            return stopWatch.ElapsedMilliseconds;
        }
    }
}