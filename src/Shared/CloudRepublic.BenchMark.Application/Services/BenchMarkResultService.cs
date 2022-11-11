using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Data;

namespace CloudRepublic.BenchMark.Application.Services
{
    public class BenchMarkResultService : IBenchMarkResultService
    {
        private readonly IBenchMarkResultRepository _benchMarkResultRepository;

        public BenchMarkResultService(IBenchMarkResultRepository benchMarkResultRepository)
        {
            _benchMarkResultRepository = benchMarkResultRepository;
        }

        public DateTimeOffset GetDateTimeNow()
        {
            return DateTimeOffset.Now;
        }

        public async Task<IEnumerable<BenchMarkResult>> GetBenchMarkResultsAsync(CloudProvider cloudProvider, HostEnvironment hostingEnvironment,
            Runtime runtime, Language language, string sku, DateTimeOffset afterDate)
        {
            var days = GetDatesBetween(afterDate, GetDateTimeNow());

            var benchMarkResults = new List<BenchMarkResult>();
            foreach (var day in days)
            {
                var monthResults = await _benchMarkResultRepository
                    .GetBenchMarkResultsAsync(cloudProvider, hostingEnvironment, runtime, language, sku, day.Year, day.Month, day.Day);
                
                benchMarkResults.AddRange(monthResults);
            }

            return benchMarkResults.Where(r => r.CreatedAt >= afterDate);
        }

        private static List<DateTimeOffset> GetDatesBetween(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var allDates = new List<DateTimeOffset>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates;

        }
    }
}