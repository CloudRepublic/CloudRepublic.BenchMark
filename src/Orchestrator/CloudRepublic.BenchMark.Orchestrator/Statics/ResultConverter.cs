using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudRepublic.BenchMark.Orchestrator.Statics;

public static class ResultConverter
{
    public static List<BenchMarkResult> ConvertToResultObject(
        IEnumerable<BenchMarkResponse> benchMarkResponses,
        BenchMarkType benchMarkType, bool isColdRequest)
    {
        return benchMarkResponses.Select(benchMarkResponse => new BenchMarkResult
        {
            Id = Guid.NewGuid().ToString(),
            CloudProvider = benchMarkType.CloudProvider,
            HostingEnvironment = benchMarkType.HostEnvironment,
            Runtime = benchMarkType.Runtime,
            Language = benchMarkType.Language,
            Success = benchMarkResponse.Success,
            RequestDuration = Convert.ToInt32(benchMarkResponse.Duration),
            IsColdRequest = isColdRequest,
            CreatedAt = DateTimeOffset.UtcNow,
        }).ToList();
    }
}