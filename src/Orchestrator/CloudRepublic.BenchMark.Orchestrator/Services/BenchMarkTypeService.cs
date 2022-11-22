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

namespace CloudRepublic.BenchMark.Orchestrator.Services;

public class BenchMarkTypeService : IBenchMarkTypeService
{
    private readonly IBenchMarkService _benchMarkService;
    private readonly IBenchMarkResultRepository _benchMarkResultRepository;
    private readonly IEnumerable<BenchMarkType> _benchMarkTypes;

    public BenchMarkTypeService(
        IBenchMarkService benchMarkService, 
        IBenchMarkResultRepository benchMarkResultRepository,
        IEnumerable<BenchMarkType> benchMarkTypes)
    {
        _benchMarkService = benchMarkService;
        _benchMarkResultRepository = benchMarkResultRepository;
        _benchMarkTypes = benchMarkTypes;
    }
    
    public async Task<List<BenchMarkResult>> RunBenchMarksAsync(int coldCalls = 5, int warmCalls = 10, int delayBetweenCalls = 30)
    {
        var results = new List<BenchMarkResult>();

        foreach (var benchMarkType in _benchMarkTypes)
        {
            var tasksCold = new List<Task<BenchMarkResponse>>();
            var tasksWarm = new List<Task<BenchMarkResponse>>();

            for (var i = 0; i < coldCalls; i++)
            {
                tasksCold.Add(_benchMarkService.RunBenchMarkAsync(benchMarkType, i));
            }

            var coldResponses = await Task.WhenAll(tasksCold);

            await Task.Delay(TimeSpan.FromSeconds(delayBetweenCalls));

            for (var i = 0; i < warmCalls; i++)
            {
                tasksWarm.Add(_benchMarkService.RunBenchMarkAsync(benchMarkType, i + coldCalls));
            }

            var warmResponses = await Task.WhenAll(tasksWarm);

            results.AddRange(ResultConverter.ConvertToResultObject(coldResponses, warmResponses, benchMarkType));
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