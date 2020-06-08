using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Statics;
using CloudRepublic.BenchMark.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Orchestrator.Services
{
    public class BenchMarkTypeService : IBenchMarkTypeService
    {
        private readonly IBenchMarkService _benchMarkService;
        private readonly BenchMarkDbContext _dbContext;

        public BenchMarkTypeService(IBenchMarkService benchMarkService, BenchMarkDbContext dbContext)
        {
            _benchMarkService = benchMarkService;
            _dbContext = dbContext;
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
                    tasksCold.Add(_benchMarkService.RunBenchMarkAsync(benchMarkType.ClientName));
                }

                await Task.WhenAll(tasksCold);

                await Task.Delay(TimeSpan.FromSeconds(delayBetweenCalls));

                for (int i = 0; i < warmCalls; i++)
                {
                    tasksWarm.Add(_benchMarkService.RunBenchMarkAsync(benchMarkType.ClientName));
                }

                await Task.WhenAll(tasksWarm);

                results.AddRange(ResultConverter.ConvertToResultObject(tasksCold.Select(t => t.Result), benchMarkType, true));
                results.AddRange(ResultConverter.ConvertToResultObject(tasksWarm.Select(t => t.Result), benchMarkType, false));

            }
            return results;

        }

        public async Task StoreBenchMarkResultsAsync(IEnumerable<BenchMarkResult> results)
        {
            if (!results.Any())
            {
                return;
            }

            foreach (var result in results)
            {
                _dbContext.BenchMarkResult.Add(result);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
