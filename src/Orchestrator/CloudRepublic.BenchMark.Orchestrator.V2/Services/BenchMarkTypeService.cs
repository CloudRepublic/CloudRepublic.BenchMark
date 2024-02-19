using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Data;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.V2.Statics;

namespace CloudRepublic.BenchMark.Orchestrator.V2.Services;

public class BenchMarkTypeService(
    IBenchMarkService benchMarkService,
    IBenchMarkResultRepository benchMarkResultRepository,
    IEnumerable<BenchMarkType> benchMarkTypes)
    : IBenchMarkTypeService
{
    public async Task<List<BenchMarkResult>> RunBenchMarksAsync(int coldCalls = 5, int warmCalls = 10, int delayBetweenCalls = 30)
    {
        var results = new List<BenchMarkResult>();

        foreach (var benchMarkType in benchMarkTypes)
        {
            var tasksCold = new List<Task<BenchMarkResponse>>();
            var tasksWarm = new List<Task<BenchMarkResponse>>();

            for (var i = 0; i < coldCalls; i++)
            {
                tasksCold.Add(benchMarkService.RunBenchMarkAsync(benchMarkType, i));
            }

            var coldResponses = await Task.WhenAll(tasksCold);

            await Task.Delay(TimeSpan.FromSeconds(delayBetweenCalls));

            for (var i = 0; i < warmCalls; i++)
            {
                tasksWarm.Add(benchMarkService.RunBenchMarkAsync(benchMarkType, i + coldCalls));
            }

            var warmResponses = await Task.WhenAll(tasksWarm);

            results.AddRange(ResultConverter.ConvertToResultObject(coldResponses, warmResponses, benchMarkType));
        }
        return results;
    }

    public async Task StoreBenchMarkResultsAsync(IEnumerable<BenchMarkResult> results)
    {
        var benchMarkResults = results as BenchMarkResult[] ?? results.ToArray();
        if (benchMarkResults.Length == 0)
        {
            return;
        }

        foreach (var result in benchMarkResults)
        {
            await benchMarkResultRepository.AddBenchMarkResultAsync(result);
        }
    }
}