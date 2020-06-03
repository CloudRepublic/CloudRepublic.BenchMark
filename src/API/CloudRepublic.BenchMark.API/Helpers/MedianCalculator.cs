using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudRepublic.BenchMark.API.Helpers
{
    public class MedianCalculator
    {
        public static BenchmarkMedians Calculate(DateTime currentDate,
            List<BenchMarkResult> dataPoints)
        {
            var dataPointsCurrentDate =
                dataPoints.Where(c => c.CreatedAt.Date == currentDate.Date);

            var dataPointsPreviousDate = dataPoints.Where(c =>
                c.CreatedAt.Date == currentDate - TimeSpan.FromDays(1));

            var currentDateMedian =
                Math.Round(dataPointsCurrentDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0);

            var medianPreviousDate =
                Math.Round(dataPointsPreviousDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0);

            return new BenchmarkMedians()
            {
                CurrentDay = currentDateMedian,
                PreviousDay = medianPreviousDate
            };
        }
    }
}