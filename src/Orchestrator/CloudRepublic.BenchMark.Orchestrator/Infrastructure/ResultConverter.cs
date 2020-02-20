using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public static class ResultConverter
    {
        public static IEnumerable<BenchMarkResult> ConvertToResultObject(
            IEnumerable<BenchMarkResponse> benchMarkResponses,
            BenchMarkType benchMarkType,bool isColdRequest)
        {
            return benchMarkResponses.Select(benchMarkResponse => new BenchMarkResult
            {
                CloudProvider = (int) benchMarkType.CloudProvider,
                HostingEnvironment = (int) benchMarkType.HostEnvironment,
                Runtime = (int) benchMarkType.Runtime,
                RunTimeVersion =  benchMarkType.RuntimeVersion,
                Success = benchMarkResponse.Success,
                RequestDuration = Convert.ToInt32(benchMarkResponse.Duration),
                IsColdRequest = isColdRequest
            }).ToList();
        }
    }
}