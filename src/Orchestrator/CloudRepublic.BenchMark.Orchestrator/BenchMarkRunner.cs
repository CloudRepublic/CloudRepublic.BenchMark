using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Extentions;
using CloudRepublic.BenchMark.Orchestrator.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class BenchMarkRunner
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BenchMarkRunner(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName("BenchmarkRunner")]
        public async Task<BenchMarkResponse> Benchmark([ActivityTrigger] BenchMarkType benchMarkType, ILogger log)
        {
            var client = _httpClientFactory.CreateBenchMarkClient(benchMarkType);
            long result = 0;
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var response = await client.GetAsync($"api/Trigger?name=BenchMark&code={Environment.GetEnvironmentVariable("AzureWindowsCsharpKey")}");
                result = stopWatch.ElapsedMilliseconds;

                return response.IsSuccessStatusCode
                    ? new BenchMarkResponse(true, result)
                    : new BenchMarkResponse(false, result);
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
                return new BenchMarkResponse(false, result);
            }
        }
    }
}