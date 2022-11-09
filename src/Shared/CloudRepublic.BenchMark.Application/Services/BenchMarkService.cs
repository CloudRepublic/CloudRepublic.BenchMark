using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Application.Services
{
    public class BenchMarkService : IBenchMarkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BenchMarkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<BenchMarkResponse> RunBenchMarkAsync(BenchMarkType benchMarkType)
        {
            var client = _httpClientFactory.CreateClient(benchMarkType.Name);
            try
            {
                var stopWatch = Stopwatch.StartNew();
                client.DefaultRequestHeaders.Add(benchMarkType.AuthenticationHeaderName, benchMarkType.AuthenticationHeaderValue);
                var response = await client.GetAsync(benchMarkType.TestEndpoint, new HttpCompletionOption());
                var result = stopWatch.ElapsedMilliseconds;


                return new BenchMarkResponse(response.IsSuccessStatusCode, result);
            }
            catch (Exception)
            {
                return new BenchMarkResponse(false, 0);
            }
        }
    }
}