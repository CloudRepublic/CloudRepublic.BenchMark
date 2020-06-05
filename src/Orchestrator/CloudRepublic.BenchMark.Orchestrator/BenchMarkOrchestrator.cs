using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Orchestrator
{
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

            await RunAndHandleAllBenchMarks();

        }

        /// <summary>
        /// This function we can test/moq without having to mess with the TimerTrigger/TimerInfo.
        /// </summary>
        /// <returns></returns>
        public async Task RunAndHandleAllBenchMarks()
        {
            var benchMarkTypes = _benchMarkTypeService.GetAllTypes();

            var benchMarkResults = await _benchMarkTypeService.RunBenchMarksAsync(benchMarkTypes);
            if (benchMarkResults.Any())
            {
                await _benchMarkTypeService.StoreBenchMarkResultsAsync(benchMarkResults);
            }
        }
    }
}