using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CloudRepublic.BenchMark.Application.Tests
{
    public class BenchMarkResultServiceTests
    {
        protected DbContextOptions<BenchMarkDbContext> ContextOptions { get; }

        public BenchMarkResultServiceTests()
        {
            ContextOptions = new DbContextOptionsBuilder<BenchMarkDbContext>()
                .UseInMemoryDatabase("TestingDB")
                .Options;
        }

        [Fact]
        public async Task StoreBenchMarkResultsAsync_Should_Add_Each_Result()
        {
            #region Arrange

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {

                    });
                context.SaveChanges();

                var itms = context.BenchMarkResult.ToList();
                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResults(CloudProvider.Azure, HostEnvironment.Linux, Runtime.Csharp, 0);

                #endregion

                #region Assert
                Assert.Equal(4, results.Count);

                #endregion
            }

            #endregion

        }
    }
}
