using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudProvider = CloudRepublic.BenchMark.Domain.Enums.CloudProvider;
using HostingEnvironment = CloudRepublic.BenchMark.Domain.Enums.HostEnvironment;
using Runtime = CloudRepublic.BenchMark.Domain.Enums.Runtime;

namespace CloudRepublic.BenchMark.API.Infrastructure
{
    public class ResponseConverter : IResponseConverter
    {
        public BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints)
        {
            var benchmarkData = new BenchMarkData()
            {
                CloudProvider = Enum.ToObject(typeof(CloudProvider), resultDataPoints.First().CloudProvider).ToString(),
                HostingEnvironment =
                    Enum.ToObject(typeof(HostingEnvironment), resultDataPoints.First().HostingEnvironment).ToString(),
                Runtime = Enum.ToObject(typeof(Runtime), resultDataPoints.First().Runtime).ToString(),
                AverageExecutionTime = Convert.ToInt32(Math.Round(resultDataPoints.Average(c => c.RequestDuration), 0))
            };

            foreach (var dataPoint in resultDataPoints.Where(c=>c.Success))
            {
                if (dataPoint.IsColdRequest)
                {
                    benchmarkData.ColdDataPoints.Add(new DataPoint(dataPoint.CreatedAt, dataPoint.RequestDuration));
                }
                else
                {
                    benchmarkData.HotDataPoints.Add(new DataPoint(dataPoint.CreatedAt, dataPoint.RequestDuration));
                }
            }

            return benchmarkData;
        }
    }
}