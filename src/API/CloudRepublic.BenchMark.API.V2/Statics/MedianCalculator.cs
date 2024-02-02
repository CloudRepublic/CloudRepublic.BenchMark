using CloudRepublic.BenchMark.API.V2.Mappers;
using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.API.V2.Statics;

public static class MedianCalculator
{
    public static BenchmarkMedians Calculate(DateTime currentDate,
        List<BenchMarkResult> dataPoints)
    {
        var dataPointsCurrentDate = dataPoints.Where(c => c.CreatedAt.Date == currentDate.Date).ToList();

        var dataPointsPreviousDate = dataPoints.Where(c => c.CreatedAt.Date == (currentDate - TimeSpan.FromDays(1)).Date).ToList();

        var currentDateMedian = dataPointsCurrentDate.Any()
            ? Math.Round(dataPointsCurrentDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0)
            : 0;

        var medianPreviousDate = dataPointsPreviousDate.Any()
            ? Math.Round(dataPointsPreviousDate.Select(c => Convert.ToDouble(c.RequestDuration)).Median(), 0)
            : 0;

        return new BenchmarkMedians {
            CurrentDay = currentDateMedian,
            PreviousDay = medianPreviousDate
        };
    }
}