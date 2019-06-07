using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Application.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using CloudRepublic.BenchMark.Orchestrator.Models;
using CloudRepublic.BenchMark.Orchestrator.Persistence;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class Orchestrator
    {
        [FunctionName("BenchMarkOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context, ILogger log)
        {
            var benchMarkTypes = BenchMarkTypeGenerator.Generate();
            foreach (var benchMarkType in benchMarkTypes)
            {
                var tasks = new List<Task<BenchMarkResponse>>();

                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(context.CallActivityAsync<BenchMarkResponse>("BenchmarkRunner", benchMarkType));
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