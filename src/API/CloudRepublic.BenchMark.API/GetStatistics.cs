using System;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.API;

public class GetStatistics
{
    private readonly IBenchMarkResultService _benchMarkResultService;
    private readonly IResponseConverterService _responseConverterService;

    public GetStatistics(IBenchMarkResultService benchMarkResultService, IResponseConverterService responseConverterService)
    {
        _benchMarkResultService = benchMarkResultService;
        _responseConverterService = responseConverterService;
    }

    [FunctionName("GetStatistics")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics")] HttpRequest req, ILogger log)
    {
        var cloudProvider = new EnumFromString<CloudProvider>(req.Query["cloudProvider"]);
        var hostingEnvironment = new EnumFromString<HostEnvironment>(req.Query["hostingEnvironment"]);
        var runtime = new EnumFromString<Runtime>(req.Query["runtime"]);
        var language = new EnumFromString<Language>(req.Query["language"]);
            
        if (!cloudProvider.ParsedSuccesfull)
            return new BadRequestObjectResult($"cloudProvider \"{ cloudProvider.StringValue }\" is not a valid value");
        
        if (!hostingEnvironment.ParsedSuccesfull)
            return new BadRequestObjectResult($"hostingEnvironment \"{ hostingEnvironment.StringValue }\" is not a valid value");
        
        if (!runtime.ParsedSuccesfull)
            return new BadRequestObjectResult($"runtime \"{ runtime.StringValue }\" is not a valid value");
        
        if (!language.ParsedSuccesfull)
            return new BadRequestObjectResult($"language \"{ language.StringValue }\" is not a valid value");

        var dayRange = Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange"));
        var currentDate = _benchMarkResultService.GetDateTimeNow();
        var resultsSinceDate = (currentDate - TimeSpan.FromDays(dayRange));

        log.LogInformation($"retrieving statistics for cloudProvider: {cloudProvider.Value}, hostingEnvironment: {hostingEnvironment.Value}, runtime: {runtime.Value}, language: {language.Value}, since: {resultsSinceDate}");
        
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