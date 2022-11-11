using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Mappers;
using CloudRepublic.BenchMark.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudRepublic.BenchMark.API;

public class GetMeasurementCategories
{
    private readonly IEnumerable<BenchMarkType> _benchMarkTypes;

    public GetMeasurementCategories(IEnumerable<BenchMarkType> benchMarkTypes)
    {
        _benchMarkTypes = benchMarkTypes;
    }

    [FunctionName("GetMeasurementCategories")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories")] HttpRequest req, ILogger log)
    {
        return new OkObjectResult(_benchMarkTypes.ToCategories());
    }
}