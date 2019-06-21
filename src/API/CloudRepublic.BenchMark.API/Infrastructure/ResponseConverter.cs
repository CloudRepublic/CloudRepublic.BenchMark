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
                Runtime = Enum.ToObject(typeof(Runtime), resultDataPoints.First().Runtime).ToString()
            };


            var currentDate = resultDataPoints.OrderByDescending(c => c.CreatedAt).First().CreatedAt.Date;
            var averageCurrentDate = resultDataPoints.Where(c => c.CreatedAt.Date == currentDate.Date)
                .Average(c => c.RequestDuration);

            benchmarkData.AverageExecutionTime = Convert.ToInt32(Math.Round(averageCurrentDate, 0));

            var previousDate = currentDate - TimeSpan.FromDays(1);

            var dataPointsCurrentDate = resultDataPoints.Where(c => c.CreatedAt.Date == currentDate.Date);
            var dataPointsPreviousDate = resultDataPoints.Where(c => c.CreatedAt.Date == previousDate.Date);

            if (dataPointsCurrentDate.Any() && dataPointsPreviousDate.Any())
            {
                var averagePreviousDate = resultDataPoints.Where(c => c.CreatedAt.Date == previousDate.Date)
                    .Average(c => c.RequestDuration);

                var difference =
                    Math.Round(((averageCurrentDate - averagePreviousDate) / Math.Abs(averageCurrentDate)) * 100, 2);
                benchmarkData.PreviousDayDifference = difference;
                benchmarkData.PreviousDayPositive = benchmarkData.PreviousDayDifference < 0;
            }

            foreach (var dataPoint in resultDataPoints.Where(c => c.Success))
            {
                if (dataPoint.IsColdRequest)
                {
                    benchmarkData.ColdDataPoints.Add(new DataPoint(dataPoint.CreatedAt.ToString("R"), dataPoint.RequestDuration));
                }
                else
                {
                    benchmarkData.WarmDataPoints.Add(new DataPoint(dataPoint.CreatedAt.ToString("R"), dataPoint.RequestDuration));
                }
            }

            return benchmarkData;
        }
    }
}