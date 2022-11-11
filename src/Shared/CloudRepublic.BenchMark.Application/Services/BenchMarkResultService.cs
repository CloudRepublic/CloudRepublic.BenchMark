using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;
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

        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }

        public async IAsyncEnumerable<BenchMarkResult> GetBenchMarkResultsAsync(CloudProvider cloudProvider, HostEnvironment hostingEnvironment,
            Runtime runtime, Language language, DateTime afterDate)
        {
            var months = GetMonthsBetween(afterDate, GetDateTimeNow());

            foreach (var month in months)
            {
                var monthResults = _benchMarkResultRepository
                    .GetBenchMarkResultsAsync(cloudProvider, hostingEnvironment, runtime, language, month.Year, month.Month);

                await foreach (var result in monthResults)
                {
                    yield return result;
                }
            }
        }

        private IEnumerable<DateOnly> GetMonthsBetween(DateTime afterDate, DateTime getDateTimeNow)
        {
            var months = new List<DateOnly>();
            var currentMonth = afterDate.Month;
            var currentYear = afterDate.Year;
            
            while (currentMonth != getDateTimeNow.Month || currentYear != getDateTimeNow.Year)
            {
                months.Add(new DateOnly(currentYear, currentMonth, 1));
                currentMonth++;
                
                if (currentMonth <= 12)
                {
                    continue;
                }
                
                currentMonth = 1;
                currentYear++;
            }
            
            months.Add(new DateOnly(currentYear, currentMonth, 1));
            return months;
        }
    }
}