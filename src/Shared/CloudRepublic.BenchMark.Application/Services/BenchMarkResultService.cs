using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using Dapper;

namespace CloudRepublic.BenchMark.Application.Services
{
    public class BenchMarkResultService : IBenchMarkResultService
    {
        private readonly IDbConnection _dbConnection;

        public BenchMarkResultService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<BenchMarkResult>> GetBenchMarkResults(string cloudProvider,
            string hostingEnvironment, string runtime, int dayRange)
        {
            var parsedCloudProvider = Enum.Parse(typeof(CloudProvider), cloudProvider);
            var parsedHostingEnvironment = Enum.Parse(typeof(HostEnvironment), hostingEnvironment);
            var parsedRuntime = Enum.Parse(typeof(Runtime), runtime);

            var results = await _dbConnection.QueryAsync<BenchMarkResult>(
                "SELECT * FROM [dbo].[BenchMarkResult] WHERE CloudProvider=@CloudProvider AND HostingEnvironment=@HostingEnvironment AND Runtime=@Runtime AND  CreatedAt >= DATEADD(DAY,-@DayRange,GETDATE()) order by CreatedAt",
                new
                {
                    CloudProvider = (int) parsedCloudProvider, HostingEnvironment = (int) parsedHostingEnvironment,
                    Runtime = (int) parsedRuntime, DayRange = dayRange
                });

            return results.ToList();
        }
    }
}