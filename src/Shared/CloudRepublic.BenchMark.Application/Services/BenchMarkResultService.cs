using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Application.Services
{
    public class BenchMarkResultService : IBenchMarkResultService
    {
        private readonly BenchMarkDbContext _dbContext;

        public BenchMarkResultService(BenchMarkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
        public async Task<List<BenchMarkResult>> GetBenchMarkResultsAsync(CloudProvider cloudProvider,
            HostEnvironment hostingEnvironment, Runtime runtime, DateTime afterDate)
        {

            var results = await _dbContext.BenchMarkResult
                .Where(result => result.CloudProvider == cloudProvider)
                .Where(result => result.HostingEnvironment == hostingEnvironment)
                .Where(result => result.Runtime == runtime)
                .Where(result => result.CreatedAt.Date >= afterDate.Date)
                .OrderByDescending(result => result.CreatedAt)
                .ToListAsync();

            return results;
        }
    }
}