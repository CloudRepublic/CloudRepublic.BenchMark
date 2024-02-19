using System.Net;
using System.Text.Json;
using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Serializers;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CloudRepublic.BenchMark.API.V2;

public class GetStatistics(IResponseCacheService responseCacheService)
{
    [Function("GetStatistics")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var cloudProvider = new EnumFromString<CloudProvider>(req.Query["cloudProvider"]);
        var hostingEnvironment = new EnumFromString<HostEnvironment>(req.Query["hostingEnvironment"]);
        var runtime = new EnumFromString<Runtime>(req.Query["runtime"]);
        var language = new EnumFromString<Language>(req.Query["language"]);
        var sku = req.Query["sku"];

        var validationErrors = new List<string>();
        if (!cloudProvider.ParsedSuccesfull)
        {
            validationErrors.Add($"cloudProvider \"{ cloudProvider.StringValue }\" is not a valid value");
        }

        if (!hostingEnvironment.ParsedSuccesfull)
            validationErrors.Add($"hostingEnvironment \"{ hostingEnvironment.StringValue }\" is not a valid value");
        
        if (!runtime.ParsedSuccesfull)
            validationErrors.Add($"runtime \"{ runtime.StringValue }\" is not a valid value");
        
        if (!language.ParsedSuccesfull)
            validationErrors.Add($"language \"{ language.StringValue }\" is not a valid value");

        if (validationErrors.Count != 0)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "application/json");
            await errorResponse.WriteAsJsonAsync(validationErrors);
        }
        
        var serializedData = await responseCacheService.RunBenchMarksAsync(cloudProvider.Value, hostingEnvironment.Value, runtime.Value, language.Value, sku);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(serializedData);

        return response;
    }
}