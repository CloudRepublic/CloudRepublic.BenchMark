using System;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

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

            var cloudProvider = req.Query["cloudProvider"];
            if(!cloudProvider.Any()) return new BadRequestResult();
            
            var hostingEnvironment = req.Query["hostingEnvironment"];
            if (!hostingEnvironment.Any()) return new BadRequestResult();
            
            var runtime = req.Query["runtime"];
            if(!hostingEnvironment.Any()) return new BadRequestResult();
            
            var benchMarkDataPoints =
                await _benchMarkResultService.GetBenchMarkResults(cloudProvider.First(),hostingEnvironment.First(),runtime.First(),
                    Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange")));
            

            return new OkObjectResult(_responseConverter.ConvertToBenchMarkData(benchMarkDataPoints));
        }
    }
}