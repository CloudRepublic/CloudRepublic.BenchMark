using System;
using System.Net;
using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.API.V2.Serializers;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.API.V2;

public class Precache(
    IEnumerable<BenchMarkType> benchMarkTypes,
    IBenchMarkResultService benchMarkResultService,
    IResponseConverterService responseConverterService,
    IResponseCacheService responseCacheService)
{
    [Function("Precache")]
    public async Task RunAsync([TimerTrigger("0 15 * * * *")] TimerInfo myTimer)
    {
        foreach (var benchMarkType in benchMarkTypes)
        {
            var benchMarkData = await GetData(
                benchMarkType.CloudProvider,
                benchMarkType.HostEnvironment,
                benchMarkType.Runtime,
                benchMarkType.Language,
                benchMarkType.Sku
            );

            if (benchMarkData != null)
            {
                await responseCacheService.StoreBenchMarkResultAsync(benchMarkData);
            }
        }
    }
    
    private async Task<BenchMarkData?> GetData(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, Language language, string sku)
    {
        var dayRange = Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange"));
        var currentDate = benchMarkResultService.GetDateTimeNow();
        var resultsSinceDate = (currentDate - TimeSpan.FromDays(dayRange));

        // IMPORTANT: ToListAsync has a potential deadlock when not provided with a cancellation token
        var benchMarkDataPoints = await benchMarkResultService.GetBenchMarkResultsAsync(
            cloudProvider,
            hostingEnvironment,
            runtime,
            language,
            sku,
            resultsSinceDate
        );
        
        var benchMarkPointsToReturn = benchMarkDataPoints.Where(c => c.Success).ToList();
        return !benchMarkPointsToReturn.Any() ? null : responseConverterService.ConvertToBenchMarkData(benchMarkPointsToReturn);
    }
}