using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class BenchMarkOrchestrator
    {
        private readonly IBenchMarkService _benchMarkService;

        public BenchMarkOrchestrator(IBenchMarkService benchMarkService)
        {
            _benchMarkService = benchMarkService;
        }

        [FunctionName("BenchMarkOrchestrator")]
        public async Task Run([TimerTrigger("0 0 */4 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

            var benchMarkTypes = BenchMarkTypeGenerator.Generate();
            foreach (var benchMarkType in benchMarkTypes)
            {
                var tasks = new List<Task<BenchMarkResponse>>();

                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(_benchMarkService.RunBenchMark(benchMarkType));
                }

                await Task.WhenAll(tasks);

                var resultsWindows =
                    ResultConverter.ConvertToResultObject(tasks.Select(t => t.Result), benchMarkType);

                using (var dbContext =
                    BenchMarkDbContextFactory.Create(Environment.GetEnvironmentVariable("BenchMarkDatabase")))
                {
                    foreach (var result in resultsWindows)
                    {
                        dbContext.BenchMarkResult.Add(result);
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}