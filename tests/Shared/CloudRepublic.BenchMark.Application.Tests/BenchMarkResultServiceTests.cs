using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task GetBenchMarkResultsAsync_Should_Return_empty_List_On_Empty_Db()
        {
            #region Arrange

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_2, new DateTime(2020, 2, 2));

                #endregion

                #region Assert

                Assert.NotNull(results);
                Assert.Empty(results);

                #endregion
            }

            #endregion

        }
        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Item_With_all_Properties_Set()
        {
            #region Arrange

            var requestAndResultDate = new DateTime(2020, 2, 2);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(requestAndResultDate),
                        RequestDuration = 18,
                        IsColdRequest = true,
                        Success = true,
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_3, requestAndResultDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results);
                var validationModel = results[0];
                Assert.Equal(12, validationModel.Id);
                Assert.Equal(CloudProvider.Azure, validationModel.CloudProvider);
                Assert.Equal(HostEnvironment.Linux, validationModel.HostingEnvironment);
                Assert.Equal(Language.Csharp, validationModel.Language);
                Assert.Equal(AzureRuntimeVersion.Version_3, validationModel.AzureRuntimeVersion);
                Assert.Equal(new DateTimeOffset(new DateTime(2020, 2, 2)), validationModel.CreatedAt);
                Assert.Equal(18, validationModel.RequestDuration);
                Assert.True(validationModel.IsColdRequest);
                Assert.True(validationModel.Success);

                #endregion
            }

            #endregion
        }

        [Theory] // testing with DateTime(2020, 2, 2)
        [InlineData(2020, 2, 2)] // matches the date
        [InlineData(2020, 2, 3)] // next day
        [InlineData(2020, 3, 2)] // next month
        [InlineData(2021, 2, 2)] // next year
        public async Task GetBenchMarkResultsAsync_Should_Only_Return_Items_On_Or_After_given_afterDate_Date(int year, int month, int day)
        {
            #region Arrange

            var requestDate = new DateTime(2020, 2, 2);
            var invalidResultDate = new DateTime(2020, 1, 1); // earlier the date
            var validResultDate = new DateTime(year, month, day);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(validResultDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 55,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(invalidResultDate),
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_3, requestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }
        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Items_On_Given_afterDate_Date_Regardless_Of_Time()
        {
            #region Arrange

            var requestDate = new DateTime(2020, 2, 2, 5, 5, 5);  // has time: 5.5.5
            var resultDate = new DateTime(2020, 2, 2, 3, 3, 3); // has time:  3.3.3 which is earlier than the request date

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(resultDate),
                    });

                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_3, requestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }

        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Items_by_CloudProvider()
        {
            #region Arrange

            var resultAndRequestDate = new DateTime(2020, 2, 2);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure, // Requested
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 55,
                        CloudProvider = CloudProvider.Firebase, //  Different
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_2, resultAndRequestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }
        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Items_by_HostingEnvironment()
        {
            #region Arrange

            var resultAndRequestDate = new DateTime(2020, 2, 2);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,// Requested
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 55,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Windows,//  Different
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_3, resultAndRequestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }
        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Items_by_Language()
        {
            #region Arrange

            var resultAndRequestDate = new DateTime(2020, 2, 2);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,// Requested
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 55,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Java,//  Different
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 34,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Nodejs,//  Different
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_2, resultAndRequestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }
        [Fact]
        public async Task GetBenchMarkResultsAsync_Should_Return_Items_By_AzureRuntimeVersion()
        {
            #region Arrange

            var resultAndRequestDate = new DateTime(2020, 2, 2);

            using (var context = new BenchMarkDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 12,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_3, // Requested
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 55,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2, //  Different
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });

                context.BenchMarkResult.Add(
                    new BenchMarkResult()
                    {
                        Id = 34,
                        CloudProvider = CloudProvider.Azure,
                        HostingEnvironment = HostEnvironment.Linux,
                        Language = Language.Csharp,
                        AzureRuntimeVersion = AzureRuntimeVersion.Version_2,//  Different
                        CreatedAt = new DateTimeOffset(resultAndRequestDate),
                    });
                context.SaveChanges();

                var testService = new BenchMarkResultService(context);

                #region Act

                var results = await testService.GetBenchMarkResultsAsync(CloudProvider.Azure, HostEnvironment.Linux, Language.Csharp, AzureRuntimeVersion.Version_3, resultAndRequestDate);

                #endregion

                #region Assert

                Assert.NotEmpty(results); // we need a result
                Assert.Single(results); // we need a single result
                Assert.Equal(12, results[0].Id); // it must be the proper result

                #endregion
            }
            #endregion

        }

    }
}
