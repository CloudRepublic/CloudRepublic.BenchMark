using System.Net;
using System.Text.Json;
using CloudRepublic.BenchMark.API.V2.Mappers;
using CloudRepublic.BenchMark.API.V2.Serializers;
using CloudRepublic.BenchMark.Application.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CloudRepublic.BenchMark.API.V2;

public class GetMeasurementCategories(IEnumerable<BenchMarkType> benchMarkTypes)
{
    [Function("GetMeasurementCategories")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        response.WriteString(JsonSerializer.Serialize(benchMarkTypes.ToCategories(), BenchMarkTypesSerializer.Default.BenchMarkType));
        return response;
        
    }
}