using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Application.Interfaces
{
    public interface IBenchMarkResultService
    {
        /// <summary>
        /// This allows us to mock DateTime.Now and as such test query that uses it.
        /// </summary>
        /// <returns></returns>
        DateTime GetToday();

        /// <summary>
        /// Grabs all stored Benchmark results
        /// </summary>
        /// <param name="cloudProvider">Results matching the given</param>
        /// <param name="hostingEnvironment">Results matching the given</param>
        /// <param name="runtime">Results matching the given</param>
        /// <param name="afterDate">Only entries which are created on or after the given Datetime.Date </param>
        /// <returns></returns>
        Task<List<BenchMarkResult>> GetBenchMarkResultsAsync(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, DateTime afterDate);
    }
}