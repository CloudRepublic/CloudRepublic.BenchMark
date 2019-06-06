using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.Orchestrator.Domain.Entities;
using CloudRepublic.BenchMark.Orchestrator.Models;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public static class ResultConverter
    {
        public static IEnumerable<BenchMarkResult> ConvertToResultObject(
            IEnumerable<BenchMarkResponse> benchMarkResponses,
            BenchMarkType benchMarkType)
        {
            return benchMarkResponses.Select(benchMarkResponse => new BenchMarkResult
                {
                    CloudProvider = (int) benchMarkType.CloudProvider,
                    HostingEnvironment = (int) benchMarkType.HostEnvironment,
                    Runtime = (int) benchMarkType.Runtime,
                    Success = benchMarkResponse.Success,
                    RequestDuration = Convert.ToInt32(benchMarkResponse.Duration)
                })
                .ToList();
        }
    }
}