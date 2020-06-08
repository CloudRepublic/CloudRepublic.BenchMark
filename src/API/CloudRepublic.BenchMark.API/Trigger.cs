using CloudRepublic.BenchMark.API.Helpers;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.API
{
    public class Trigger
    {
        private readonly IBenchMarkResultService _benchMarkResultService;
        private readonly IResponseConverter _responseConverter;

        public Trigger(IBenchMarkResultService benchMarkResultService, IResponseConverter responseConverter)
        {
            _benchMarkResultService = benchMarkResultService;
            _responseConverter = responseConverter;
        }

        [FunctionName("Trigger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            EnumFromString<CloudProvider> cloudProvider = new EnumFromString<CloudProvider>(req.Query["cloudProvider"]);
            EnumFromString<HostEnvironment> hostingEnvironment = new EnumFromString<HostEnvironment>(req.Query["hostingEnvironment"]);
            EnumFromString<Runtime> runtime = new EnumFromString<Runtime>(req.Query["runtime"]);
            if (!cloudProvider.ParsedSuccesfull || !hostingEnvironment.ParsedSuccesfull || !runtime.ParsedSuccesfull)
            {
                return new BadRequestResult();
            }

            var dayRange = Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange"));


            var benchMarkDataPoints = await _benchMarkResultService.GetBenchMarkResults(
                    cloudProvider.Value,
                    hostingEnvironment.Value,
                    runtime.Value,
                    dayRange
                    );

            var benchMarkPointsToReturn = benchMarkDataPoints.Where(c => c.Success).ToList();
            if (!benchMarkPointsToReturn.Any())
            {
                return new NotFoundResult();
            }

            var convertedData = _responseConverter.ConvertToBenchMarkData(benchMarkPointsToReturn);
            return new OkObjectResult(convertedData);
        }
    }
}