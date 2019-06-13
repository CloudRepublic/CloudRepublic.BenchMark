using System;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.API
{
    public class Trigger
    {
        private readonly IBenchMarkResultService _benchMarkResultService;
        private readonly IResponseConverter _responseConverter;

        public Trigger(IBenchMarkResultService benchMarkResultService,IResponseConverter responseConverter)
        {
            _benchMarkResultService = benchMarkResultService;
            _responseConverter = responseConverter;
        }

        [FunctionName("Trigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var benchMarkDataPoints =
                await _benchMarkResultService.GetBenchMarkResults(
                    Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange")));

            return new OkObjectResult(_responseConverter.ConvertToBenchMarkData(benchMarkDataPoints));
        }
    }
}