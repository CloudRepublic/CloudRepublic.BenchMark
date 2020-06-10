using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
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
        public async Task<BenchMarkResponse> RunBenchMarkAsync(string clientName)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var response = await client.GetAsync($"api/Trigger?name=BenchMark");
                var result = stopWatch.ElapsedMilliseconds;


                return new BenchMarkResponse(response.IsSuccessStatusCode, result);
            }
            catch
            {
                return new BenchMarkResponse(false, 0);
            }
        }
    }
}