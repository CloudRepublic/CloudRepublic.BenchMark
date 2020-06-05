using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public class BenchMarkTypeService : IBenchMarkTypeService
    {
        private readonly IBenchMarkService _benchMarkService;

        public BenchMarkTypeService(IBenchMarkService benchMarkService)
        {
            _benchMarkService = benchMarkService;
        }


        public IEnumerable<BenchMarkType> GetAllTypes()
        {
            return BenchMarkTypeGenerator.GetAllTypes();
        }

        public async Task<List<BenchMarkResult>> RunBenchMarksAsync(IEnumerable<BenchMarkType> benchMarkTypes, int coldCalls = 5, int warmCalls = 10, int delayBetweenCalls = 30)
        {
            var results = new List<BenchMarkResult>();

            foreach (var benchMarkType in benchMarkTypes)
            {
                var tasksCold = new List<Task<BenchMarkResponse>>();
                var tasksWarm = new List<Task<BenchMarkResponse>>();

                for (int i = 0; i < coldCalls; i++)
                {
                    tasksCold.Add(_benchMarkService.RunBenchMark(benchMarkType));
                }

                await Task.WhenAll(tasksCold);

                await Task.Delay(TimeSpan.FromSeconds(delayBetweenCalls));

                for (int i = 0; i < warmCalls; i++)
                {
                    tasksWarm.Add(_benchMarkService.RunBenchMark(benchMarkType));
                }

                await Task.WhenAll(tasksWarm);

                results.AddRange(ResultConverter.ConvertToResultObject(tasksCold.Select(t => t.Result), benchMarkType, true));
                results.AddRange(ResultConverter.ConvertToResultObject(tasksWarm.Select(t => t.Result), benchMarkType, false));

            }
            return results;

        }

        public async Task StoreBenchMarkResultsAsync(IEnumerable<BenchMarkResult> results)
        {
            var connectionString = Environment.GetEnvironmentVariable("BenchMarkDatabase");

            using (var dbContext = BenchMarkDbContextFactory.Create(connectionString))
            {
                foreach (var result in results)
                {
                    dbContext.BenchMarkResult.Add(result);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
