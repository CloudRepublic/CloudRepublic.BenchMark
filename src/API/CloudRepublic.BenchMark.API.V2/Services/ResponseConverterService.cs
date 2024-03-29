using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.API.V2.Statics;
using CloudRepublic.BenchMark.Data;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.API.V2.Services
{
    public class ResponseConverterService : IResponseConverterService
    {
        public BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints)
        {
            var firstResult = resultDataPoints.OrderByDescending(c => c.CreatedAt).First();
            var benchmarkData = new BenchMarkData(
                firstResult.CloudProvider.GetName(),
                firstResult.HostingEnvironment.GetName(),
                firstResult.Runtime.GetName(),
                firstResult.Language.GetName(),
                firstResult.Sku);

            var currentDate = firstResult.CreatedAt.Date;


            var coldDataPoints = resultDataPoints.Where(c => c.IsColdRequest).ToList();
            var coldMedians = MedianCalculator.Calculate(currentDate, coldDataPoints);
            benchmarkData.ColdMedianExecutionTime = coldMedians.CurrentDay;
            benchmarkData.ColdPreviousDayDifference = coldMedians.DifferencePercentage;
            benchmarkData.ColdPreviousDayPositive = benchmarkData.ColdPreviousDayDifference < 0;

            var warmDataPoints = resultDataPoints.Where(c => !c.IsColdRequest).ToList();
            var warmMedians = MedianCalculator.Calculate(currentDate, warmDataPoints);
            benchmarkData.WarmMedianExecutionTime = warmMedians.CurrentDay;
            benchmarkData.WarmPreviousDayDifference = warmMedians.DifferencePercentage;
            benchmarkData.WarmPreviousDayPositive = benchmarkData.WarmPreviousDayDifference < 0;


            var dates = new List<DateTime>();
            for (var i = 0; i < Convert.ToInt32(Environment.GetEnvironmentVariable("dayRange")); i++)
            {
                dates.Add(currentDate - TimeSpan.FromDays(i));
            }

            dates = dates.OrderBy(c => c.Date).ToList();

            foreach (var date in dates)
            {
                benchmarkData.ColdDataPoints.Add(new DataPoint
                {
                    CreatedAt = date.ToString("yyyy MMMM dd"),
                    ExecutionTimes = coldDataPoints.Where(c => c.CreatedAt.Date == date.Date)
                        .Select(c => c.RequestDuration).ToList()
                });

                benchmarkData.WarmDataPoints.Add(new DataPoint{
                    CreatedAt = date.ToString("yyyy MMMM dd"),
                    ExecutionTimes = warmDataPoints.Where(c => c.CreatedAt.Date == date.Date)
                        .Select(c => c.RequestDuration).ToList()
                    });
            }

            return benchmarkData;
        }
    }
}