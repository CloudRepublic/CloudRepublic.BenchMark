using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.API.V2.Interfaces;

public interface IResponseCacheService
{
    Task StoreBenchMarkResultAsync(BenchMarkData results);
    Task<string> RunBenchMarksAsync(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, Language language, string sku);
}