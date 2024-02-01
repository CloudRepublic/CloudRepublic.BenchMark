using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.Orchestrator.V2.Statics;

public static class ResultConverter
{
    public static List<BenchMarkResult> ConvertToResultObject(
        IEnumerable<BenchMarkResponse> coldBenchMarkResponses,
        IEnumerable<BenchMarkResponse> warmBenchMarkResponses,
        BenchMarkType benchMarkType)
    {
        var coldResults = coldBenchMarkResponses.Select(benchMarkResponse => new BenchMarkResult
        {
            Id = Guid.NewGuid().ToString(),
            CloudProvider = benchMarkType.CloudProvider,
            HostingEnvironment = benchMarkType.HostEnvironment,
            Runtime = benchMarkType.Runtime,
            Language = benchMarkType.Language,
            Sku = benchMarkType.Sku,
            Success = benchMarkResponse.Success,
            StatusCode = benchMarkResponse.StatusCode,
            RequestDuration = Convert.ToInt32(benchMarkResponse.Duration),
            IsColdRequest = true,
            IsScaleUp = false,
            ServerName = benchMarkResponse.ServerName,
            CreatedAt = DateTimeOffset.UtcNow,
            CallPositionNumber = benchMarkResponse.CallPositionNumber
        }).ToList();
        
        var warmResults = warmBenchMarkResponses.Select(benchMarkResponse =>
        {
            var newServer = coldResults.FirstOrDefault(x => x.ServerName == benchMarkResponse.ServerName) is null;
            return new BenchMarkResult
            {
                Id = Guid.NewGuid().ToString(),
                CloudProvider = benchMarkType.CloudProvider,
                HostingEnvironment = benchMarkType.HostEnvironment,
                Runtime = benchMarkType.Runtime,
                Language = benchMarkType.Language,
                Sku = benchMarkType.Sku,
                Success = benchMarkResponse.Success,
                StatusCode = benchMarkResponse.StatusCode,
                RequestDuration = Convert.ToInt32(benchMarkResponse.Duration),
                IsColdRequest = newServer,
                IsScaleUp = newServer,
                ServerName = benchMarkResponse.ServerName,
                CreatedAt = DateTimeOffset.UtcNow,
                CallPositionNumber = benchMarkResponse.CallPositionNumber
            };
        }).ToList();

        return coldResults.Concat(warmResults).OrderBy(r => r.CallPositionNumber).ToList();
    }
}