using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class BenchMarkRunner
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _client;


        public BenchMarkRunner(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName("BenchmarkRunner")]
        public async Task<long?> Benchmark([ActivityTrigger] BenchMarkType benchMarkType, ILogger log)
        {
            switch (benchMarkType.CloudProvider)
            {
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Windows &&
                                              benchMarkType.Runtime == Runtime.Csharp:
                    _client = _httpClientFactory.CreateClient("AzureWindowsCsharpClient");
                    break;
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Windows &&
                                              benchMarkType.Runtime == Runtime.Nodejs:
                    throw new NotImplementedException(nameof(benchMarkType));
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Csharp:
                    throw new NotImplementedException(nameof(benchMarkType));
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Nodejs:
                    throw new NotImplementedException(nameof(benchMarkType));
                default:
                    throw new ArgumentOutOfRangeException(nameof(benchMarkType));
            }

            try
            {
                var stopWatch = Stopwatch.StartNew();
                var response = await _client.GetAsync("api/Trigger?name=BenchMark");
                var result = stopWatch.ElapsedMilliseconds;

                if (response.IsSuccessStatusCode)
                {
                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
                return null;
            }
        }
    }
}