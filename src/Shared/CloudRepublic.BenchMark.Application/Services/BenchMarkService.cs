using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Extensions;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.Application.Services
{
    public class BenchMarkService : IBenchMarkService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BenchMarkService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<BenchMarkResponse> RunBenchMark(BenchMarkType benchMarkType)
        {
            var client = _httpClientFactory.CreateBenchMarkClient(benchMarkType);
            long result = 0;
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var response = await client.GetAsync($"api/Trigger?name=BenchMark");
                result = stopWatch.ElapsedMilliseconds;

                return response.IsSuccessStatusCode
                    ? new BenchMarkResponse(true, result)
                    : new BenchMarkResponse(false, result);
            }
            catch (Exception e)
            {
                return new BenchMarkResponse(false, result);
            }
        }
    }
}