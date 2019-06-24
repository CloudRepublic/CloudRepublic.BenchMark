using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.Domain.Entities;
using MathNet.Numerics.Statistics;

namespace CloudRepublic.BenchMark.API.Helpers
{
    public class MedianCalculator
    {
        public static (double currentDay, double previousDay) Calculate(DateTime currentDate,
            List<BenchMarkResult> dataPoints, bool useColdRequests)
        {
            var dataPointsCurrentDate =
                dataPoints.Where(c => c.CreatedAt.Date == currentDate.Date && c.IsColdRequest == useColdRequests);
            
            var dataPointsPreviousDate = dataPoints.Where(c =>
                c.CreatedAt.Date == currentDate - TimeSpan.FromDays(1) && c.IsColdRequest == useColdRequests);

            var currentDateMedian =
                Math.Round(dataPointsCurrentDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0);

            var medianPreviousDate =
                Math.Round(dataPointsPreviousDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0);

            return (currentDateMedian, medianPreviousDate);
        }
    }
}