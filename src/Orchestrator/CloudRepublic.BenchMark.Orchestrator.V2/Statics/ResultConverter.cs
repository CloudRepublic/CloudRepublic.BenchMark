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
        var serverNames = coldBenchMarkResponses.Select(x => x.ServerName).ToList();
        
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
            if (serverNames.FirstOrDefault(x => x == benchMarkResponse.ServerName) is null)
            {
                serverNames.Add(benchMarkResponse.ServerName);
                return null;
            }
            
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
                IsColdRequest = false,
                IsScaleUp = false,
                ServerName = benchMarkResponse.ServerName,
                CreatedAt = DateTimeOffset.UtcNow,
                CallPositionNumber = benchMarkResponse.CallPositionNumber
            };
        }).Where(x => x is not null).ToList();

        return coldResults.Concat(warmResults).OrderBy(r => r.CallPositionNumber).ToList();
    }
}