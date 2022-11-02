using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.API.Models;
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
        private readonly IResponseConverterService _responseConverterService;

        public Trigger(IBenchMarkResultService benchMarkResultService, IResponseConverterService responseConverterService)
        {
            _benchMarkResultService = benchMarkResultService;
            _responseConverterService = responseConverterService;
        }

        [FunctionName("Trigger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var cloudProvider = new EnumFromString<CloudProvider>(req.Query["cloudProvider"]);
            var hostingEnvironment = new EnumFromString<HostEnvironment>(req.Query["hostingEnvironment"]);
            var runtime = new EnumFromString<Runtime>(req.Query["runtime"]);
            var language = new EnumFromString<Language>(req.Query["language"]);
            
            if (!cloudProvider.ParsedSuccesfull || !hostingEnvironment.ParsedSuccesfull || !runtime.ParsedSuccesfull)
            {
                return new BadRequestResult();
            }

            var dayRange = Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange"));
            var currentDate = _benchMarkResultService.GetDateTimeNow();
            var resultsSinceDate = (currentDate - TimeSpan.FromDays(dayRange));

            // IMPORTANT: ToListAsync has a potential deadlock when not provided with a cancellation token
            var benchMarkDataPoints = await _benchMarkResultService.GetBenchMarkResultsAsync(
                    cloudProvider.Value,
                    hostingEnvironment.Value,
                    runtime.Value,
                    language.Value,
                    resultsSinceDate
                    ).ToListAsync(req.HttpContext.RequestAborted);

            var benchMarkPointsToReturn = benchMarkDataPoints.Where(c => c.Success).ToList();
            if (!benchMarkPointsToReturn.Any())
            {
                return new NotFoundResult();
            }

            var convertedData = _responseConverterService.ConvertToBenchMarkData(benchMarkPointsToReturn);
            return new OkObjectResult(convertedData);
        }
    }
}