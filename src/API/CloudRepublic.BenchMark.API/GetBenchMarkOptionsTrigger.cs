using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.API
{
    public class GetBenchMarkOptionsTrigger
    {

        private readonly IBenchMarkTypeService _benchMarkTypeService;
        private readonly IResponseConverterService _responseConverterService;

        public GetBenchMarkOptionsTrigger(IBenchMarkTypeService benchMarkTypeService, IResponseConverterService responseConverterService)
        {
            _benchMarkTypeService = benchMarkTypeService;
            _responseConverterService = responseConverterService;
        }

        [FunctionName("get-benchmarktypes")]
        public async Task<IActionResult> GetBenchMarkTypesAsync([HttpTrigger(AuthorizationLevel.
            Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger - GetBenchMarkTypes function started a request.");

            var benchMarkTypes = _benchMarkTypeService.GetAllTypes().ToList();

            var benchMarkOptions = _responseConverterService.ConvertToBenchMarkOptions(benchMarkTypes);
            return new OkObjectResult(benchMarkOptions);
        }
    }
}