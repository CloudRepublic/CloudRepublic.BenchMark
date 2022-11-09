using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Orchestrator;

public class BenchMarkOrchestrator
{
    private readonly IBenchMarkTypeService _benchMarkTypeService;

    public BenchMarkOrchestrator(IBenchMarkTypeService benchMarkTypeService)
    {
        _benchMarkTypeService = benchMarkTypeService;
    }

    [FunctionName("BenchMarkOrchestrator")]
    public async Task Run([TimerTrigger("0 0 */1 * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

        await RunAndHandleAllBenchMarksAsync();

    }

    /// <summary>
    /// This function we can test/moq without having to mess with the TimerTrigger/TimerInfo.
    /// </summary>
    /// <returns></returns>
    public async Task RunAndHandleAllBenchMarksAsync()
    {
        var benchMarkResults = await _benchMarkTypeService.RunBenchMarksAsync();
        if (benchMarkResults.Any())
        {
            await _benchMarkTypeService.StoreBenchMarkResultsAsync(benchMarkResults);
        }
    }
}