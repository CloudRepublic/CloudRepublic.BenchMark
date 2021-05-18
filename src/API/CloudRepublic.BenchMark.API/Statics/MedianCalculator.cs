using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudRepublic.BenchMark.API.Statics
{
    public class MedianCalculator
    {
        public static BenchmarkMedians Calculate(DateTime currentDate,
            List<BenchMarkResult> dataPoints)
        {
            var dataPointsCurrentDate = dataPoints.Where(c => c.CreatedAt.Date == currentDate.Date);

            var dataPointsPreviousDate = dataPoints.Where(c => c.CreatedAt.Date == (currentDate - TimeSpan.FromDays(1)).Date);

            var currentDateMedian = dataPointsCurrentDate.Any()
                ? Math.Round(dataPointsCurrentDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0)
                : 0;

            var medianPreviousDate = dataPointsPreviousDate.Any()
                ? Math.Round(dataPointsPreviousDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0)
                : 0;

            return new BenchmarkMedians()
            {
                CurrentDay = currentDateMedian,
                PreviousDay = medianPreviousDate
            };
        }
    }
}