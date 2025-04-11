using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.API.V2.Statics;

public static class OutliersCalculator
{
    public static BenchmarkOutliers Calculate(DateTime currentDate, List<BenchMarkResult> dataPoints)
    {
        if (dataPoints.Count == 0)
        {
            return new BenchmarkOutliers
            {
                OutliersPerDay = new Dictionary<DateTime, int[]> { [currentDate] = [] }
            };
        }

        var durationsCurrentDate = dataPoints.Where(dp => dp.CreatedAt.Date == currentDate)
            .Select(dp => dp.RequestDuration)
            .ToList();

        var meanCurrentDate = durationsCurrentDate.Average();
        double stdDevCurrentDate = Math.Sqrt(durationsCurrentDate.Select(x => Math.Pow(x - meanCurrentDate, 2)).Average());
        var outliersCurrentDate = durationsCurrentDate.Where(x => Math.Abs((x - meanCurrentDate) / stdDevCurrentDate) > 2).ToArray();

        var outliers = new BenchmarkOutliers
        {
            OutliersPerDay = new Dictionary<DateTime, int[]> { [currentDate] = outliersCurrentDate }
        };

        for (var i = 0; i < Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange")); i++)
        {
            var dateToCalculate = currentDate -= TimeSpan.FromDays(i);

            var durationsForDate = dataPoints.Where(dp => dp.CreatedAt.Date == dateToCalculate)
                .Select(dp => dp.RequestDuration)
                .ToList();

            if (durationsForDate.Count == 0)
            {
                continue;
            }
            var meanForDate = durationsForDate.Average();

            double stdDevForDate = Math.Sqrt(durationsForDate.Select(x => Math.Pow(x - meanForDate, 2)).Average());
            var outliersForDate = durationsForDate.Where(x => Math.Abs((x - meanForDate) / stdDevForDate) > 2).ToArray();
            outliers.OutliersPerDay[dateToCalculate] = outliersForDate;
        }

        return outliers;
    }
}