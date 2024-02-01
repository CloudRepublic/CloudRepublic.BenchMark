using System;
using CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator.V2;

public class BenchMarkOrchestrator(ILoggerFactory loggerFactory, IBenchMarkTypeService _benchMarkTypeService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<BenchMarkOrchestrator>();

    [Function("BenchMarkOrchestrator")]
    public async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

        await RunAndHandleAllBenchMarksAsync();
    }
    
    /// <summary>
    /// This function we can test/moq without having to mess with the TimerTrigger/TimerInfo.
    /// </summary>
    /// <returns></returns>
    private async Task RunAndHandleAllBenchMarksAsync()
    {
        var benchMarkResults = await _benchMarkTypeService.RunBenchMarksAsync();
        if (benchMarkResults.Any())
        {
            await _benchMarkTypeService.StoreBenchMarkResultsAsync(benchMarkResults);
        }
    }
}