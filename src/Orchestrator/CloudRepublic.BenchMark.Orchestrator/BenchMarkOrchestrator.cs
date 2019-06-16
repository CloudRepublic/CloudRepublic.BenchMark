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
                var tasksCold = new List<Task<BenchMarkResponse>>();
                var tasksWarm = new List<Task<BenchMarkResponse>>();

                for (int i = 0; i < 1; i++)
                {
                    tasksCold.Add(_benchMarkService.RunBenchMark(benchMarkType));
                }

                await Task.WhenAll(tasksCold);

                for (int i = 0; i < 10; i++)
                {
                    tasksWarm.Add(_benchMarkService.RunBenchMark(benchMarkType));
                }

                await Task.WhenAll(tasksWarm);

                var resultsCold =
                    ResultConverter.ConvertToResultObject(tasksCold.Select(t => t.Result), benchMarkType,true);
                
                var resultWarm =
                    ResultConverter.ConvertToResultObject(tasksWarm.Select(t => t.Result), benchMarkType,false);


                using (var dbContext =
                    BenchMarkDbContextFactory.Create(Environment.GetEnvironmentVariable("BenchMarkDatabase")))
                {
                    foreach (var result in resultsCold)
                    {
                        dbContext.BenchMarkResult.Add(result);
                    }

                    foreach (var result in resultWarm)
                    {
                        dbContext.BenchMarkResult.Add(result);
                    }

 
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}