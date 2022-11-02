using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Data;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Statics;

namespace CloudRepublic.BenchMark.Orchestrator.Services;

public class BenchMarkTypeService : IBenchMarkTypeService
{
    private readonly IBenchMarkService _benchMarkService;
    private readonly IBenchMarkResultRepository _benchMarkResultRepository;

    public BenchMarkTypeService(IBenchMarkService benchMarkService, IBenchMarkResultRepository benchMarkResultRepository)
    {
        _benchMarkService = benchMarkService;
        _benchMarkResultRepository = benchMarkResultRepository;
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

            for(var i = 0; i < results.Count; i++)
            {
                results[i].CallPositionNumber = i;
            }
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
            await _benchMarkResultRepository.AddBenchMarkResultAsync(result);
        }
    }
}