using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
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

        public async Task<IEnumerable<BenchMarkResult>> GetBenchMarkResults(int dayRange)
        {
            var results = await _dbConnection.QueryAsync<BenchMarkResult>(
                "SELECT * FROM [dbo].[BenchMarkResult] WHERE CreatedAt >= DATEADD(DAY,-@DayRange,GETDATE())",
                new {DayRange = dayRange});

            return results;
        }
    }
}