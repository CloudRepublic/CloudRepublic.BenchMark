using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.API.Helpers;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using MathNet.Numerics.Statistics;
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


            var coldMedians = MedianCalculator.Calculate(currentDate, resultDataPoints, true);
            benchmarkData.ColdMedianExecutionTime = coldMedians.currentDay;

            var coldDifference =
                Math.Round(
                    ((coldMedians.currentDay - coldMedians.previousDay) /
                     Math.Abs(coldMedians.currentDay)) * 100, 2);

            benchmarkData.ColdPreviousDayDifference = coldDifference;
            benchmarkData.ColdPreviousDayPositive = benchmarkData.ColdPreviousDayDifference < 0;

            var warmMedians = MedianCalculator.Calculate(currentDate, resultDataPoints, false);
            benchmarkData.WarmMedianExecutionTime = warmMedians.currentDay;

            var warmDifference =
                Math.Round(
                    ((warmMedians.currentDay - warmMedians.previousDay) /
                     Math.Abs(warmMedians.currentDay)) * 100, 2);

            benchmarkData.WarmPreviousDayDifference = warmDifference;
            benchmarkData.WarmPreviousDayPositive = benchmarkData.WarmPreviousDayDifference < 0;

            var dates = new List<DateTime>();
            for (var i = 0; i < Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange")); i++)
            {
                dates.Add(currentDate - TimeSpan.FromDays(i));
            }

            dates = dates.OrderBy(c=>c.Date).ToList();
            
            foreach (var date in dates)
            {
                benchmarkData.ColdDataPoints.Add(new DataPoint(date.ToString("yyyy MMMM dd"),
                    resultDataPoints.Where(c => c.CreatedAt.Date == date.Date && c.IsColdRequest)
                        .Select(c => c.RequestDuration).ToList()));

                benchmarkData.WarmDataPoints.Add(new DataPoint(date.ToString("yyyy MMMM dd"),
                    resultDataPoints.Where(c => c.CreatedAt.Date == date.Date && !c.IsColdRequest)
                        .Select(c => c.RequestDuration).ToList()));
            }

            return benchmarkData;
        }
    }
}