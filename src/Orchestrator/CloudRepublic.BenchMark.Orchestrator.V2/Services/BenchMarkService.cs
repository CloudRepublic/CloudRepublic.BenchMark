using System.Diagnostics;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;

namespace CloudRepublic.BenchMark.Orchestrator.V2.Services;

public class BenchMarkService : IBenchMarkService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BenchMarkService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<BenchMarkResponse> RunBenchMarkAsync(BenchMarkType benchMarkType, int CallPositionNumber)
    {
        var client = _httpClientFactory.CreateClient("benchmarkTester");
        try
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, benchMarkType.TestEndpoint);
                
            requestMessage.Headers.Add(benchMarkType.AuthenticationHeaderName, benchMarkType.AuthenticationHeaderValue);
    
            var stopWatch = Stopwatch.StartNew();
            var response = await client.SendAsync(requestMessage);
            var result = stopWatch.ElapsedMilliseconds;
                
            return new BenchMarkResponse(response.IsSuccessStatusCode, (int)response.StatusCode, result, await response.Content.ReadAsStringAsync(), CallPositionNumber);
        }
        catch (Exception)
        {
            return new BenchMarkResponse(false, 0, 0, null, CallPositionNumber);
        }
    }
}