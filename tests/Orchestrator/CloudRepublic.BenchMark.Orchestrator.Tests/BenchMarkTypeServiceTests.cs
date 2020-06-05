using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace CloudRepublic.BenchMark.Orchestrator.Tests
{
    public class BenchMarkTypeServiceTests
    {

        private readonly Mock<IBenchMarkService> _mockIBenchMarkService;

        public BenchMarkTypeServiceTests()
        {
            _mockIBenchMarkService = new Mock<IBenchMarkService>();
        }
        [Fact]
        public void GetAllTypes_Should_Return_BenchMarkType_With_All_Data()
        {
            #region Arrange

            var testService = new BenchMarkTypeService(null, null);

            #endregion

            #region Act

            var benchMarkTypes = testService.GetAllTypes();

            #endregion

            #region Assert

            Assert.NotNull(benchMarkTypes);

            // we hard coded verify only the first and last(because its so different) entries for now
            var firstTypeForTesting = benchMarkTypes.First();
            Assert.Equal(CloudProvider.Azure, firstTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Windows, firstTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.Csharp, firstTypeForTesting.Runtime);
            Assert.Equal("AzureWindowsCsharp", firstTypeForTesting.Name);
            Assert.Equal("AzureWindowsCsharpClient", firstTypeForTesting.ClientName);
            Assert.Equal("AzureWindowsCsharpKey", firstTypeForTesting.KeyName);
            Assert.Equal("AzureWindowsCsharpUrl", firstTypeForTesting.UrlName);
            Assert.True(firstTypeForTesting.SetXFunctionsKey);


            var lastTypeForTesting = benchMarkTypes.Last();
            Assert.Equal(CloudProvider.Firebase, lastTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Linux, lastTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.Nodejs, lastTypeForTesting.Runtime);
            Assert.Equal("FirebaseLinuxNodejs", lastTypeForTesting.Name);
            Assert.Equal("FirebaseLinuxNodejsClient", lastTypeForTesting.ClientName);
            Assert.Equal("FirebaseLinuxNodejsKey", lastTypeForTesting.KeyName);
            Assert.Equal("FirebaseLinuxNodejsUrl", lastTypeForTesting.UrlName);
            Assert.False(lastTypeForTesting.SetXFunctionsKey);

            #endregion
        }
        [Fact]
        public void GetAllTypes_Should_Return_BenchMarkTypes()
        {
            #region Arrange


            var testService = new BenchMarkTypeService(null, null);

            #endregion

            #region Act

            var benchMarkTypes = testService.GetAllTypes();

            #endregion

            #region Assert

            Assert.NotNull(benchMarkTypes);
            Assert.NotEmpty(benchMarkTypes);

            #endregion
        }


        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Empty_Results_On_No_BenchMarkTypes()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>();
            var testService = new BenchMarkTypeService(null, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            Assert.Empty(benchMarkResults);

            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_No_Results_For_BenchMarkTypes_when_No_Calls_Are_Asked()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needed
                new BenchMarkType()
            };
            var coldCalls = 0;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            Assert.Empty(benchMarkResults);


            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Cold_Calls_Are_Asked()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 1;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            Assert.Single(benchMarkResults);
            Assert.True(benchMarkResults[0].IsColdRequest);


            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Warm_Calls_Are_Asked()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 0;
            var warmCalls = 1;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            Assert.Single(benchMarkResults);
            Assert.False(benchMarkResults[0].IsColdRequest);


            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Cold_And_Warm_Calls_Are_Asked()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 1;
            var warmCalls = 1;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            Assert.Equal(2, benchMarkResults.Count);
            // colds are always run before the warm calls
            Assert.True(benchMarkResults[0].IsColdRequest);
            // warms come after the cold calls
            Assert.False(benchMarkResults[1].IsColdRequest);


            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_Multiple_BenchMarkTypes_When_Cold_And_Warm_Calls_Are_Asked()
        {
            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                new BenchMarkType(){
                 CloudProvider = CloudProvider.Azure,
                },
                new BenchMarkType(){
                 CloudProvider = CloudProvider.Firebase,
                }
            };
            var coldCalls = 1;
            var warmCalls = 1;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            // 2 types, ,1 cold, 1 warm = 4 total
            Assert.Equal(4, benchMarkResults.Count);

            // FOR THE AZURE TYPE
            // colds are always run before the warm calls
            Assert.True(benchMarkResults[0].IsColdRequest);
            Assert.Equal(CloudProvider.Azure, benchMarkResults[0].CloudProvider); // we need something to identify the difference so use the provider for now
            // warms come after the cold calls
            Assert.False(benchMarkResults[1].IsColdRequest);
            Assert.Equal(CloudProvider.Azure, benchMarkResults[1].CloudProvider); // we need something to identify the difference so use the provider for now

            // FOR THE FIREBASE TYPE
            // colds are always run before the warm calls
            Assert.True(benchMarkResults[2].IsColdRequest);
            Assert.Equal(CloudProvider.Firebase, benchMarkResults[2].CloudProvider); // we need something to identify the difference so use the provider for now
            // warms come after the cold calls
            Assert.False(benchMarkResults[3].IsColdRequest);
            Assert.Equal(CloudProvider.Firebase, benchMarkResults[3].CloudProvider); // we need something to identify the difference so use the provider for now

            #endregion
        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_BenchMarkResponse_as_Properly_Converted_Results()
        {

            #region Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                new BenchMarkType(){
                 Name = "TestBenchMark",
                 CloudProvider = CloudProvider.Firebase,
                 HostEnvironment = HostEnvironment.Linux,
                 Runtime = Runtime.Fsharp,
                 SetXFunctionsKey = true,
                },
            };
            var coldCalls = 1;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 567);
            _mockIBenchMarkService.Setup(service => service.RunBenchMark(It.IsAny<BenchMarkType>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);

            #endregion

            #region Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkResults);
            var validationResult = benchMarkResults[0];

            // from the benchmarktype
            Assert.True(validationResult.IsColdRequest);
            Assert.Equal(CloudProvider.Firebase, validationResult.CloudProvider);
            Assert.Equal(HostEnvironment.Linux, validationResult.HostingEnvironment);
            Assert.Equal(Runtime.Fsharp, validationResult.Runtime);
            // from the actual benchmark
            Assert.Equal(567, validationResult.RequestDuration);
            Assert.True(validationResult.Success);

            // Set by the DB
            Assert.Equal(default(DateTimeOffset), validationResult.CreatedAt);
            Assert.Equal(0, validationResult.Id);

            #endregion
        }



        [Fact]
        public async Task StoreBenchMarkResultsAsync_Should_Return_When_No_Results_and_Not_Call_DbContext()
        {
            #region Arrange

            var benchMarkResults = new List<BenchMarkResult>();
            var mockDbContext = new Mock<BenchMarkDbContext>();

            var testService = new BenchMarkTypeService(null, mockDbContext.Object);

            #endregion

            #region Act

            await testService.StoreBenchMarkResultsAsync(benchMarkResults);

            #endregion

            #region Assert

            mockDbContext.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            #endregion
        }
        [Fact]
        public async Task StoreBenchMarkResultsAsync_Should_Add_Each_Result()
        {
            #region Arrange

            var sourceList = new List<BenchMarkResult>()
            {
            };

            var mockDbContext = new Mock<BenchMarkDbContext>();
            var mockedDbSet = new Mock<DbSet<BenchMarkResult>>();
            mockedDbSet.Setup(d => d.Add(It.IsAny<BenchMarkResult>())).Callback<BenchMarkResult>((s) => sourceList.Add(s));
            mockDbContext.Setup(dbContext => dbContext.BenchMarkResult).Returns(mockedDbSet.Object);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                 new BenchMarkResult(),
                 new BenchMarkResult(),
                 new BenchMarkResult(),
                 new BenchMarkResult(),
            };

            var testService = new BenchMarkTypeService(null, mockDbContext.Object);

            #endregion

            #region Act

            await testService.StoreBenchMarkResultsAsync(benchMarkResults);

            #endregion

            #region Assert
            Assert.Equal(4, sourceList.Count);
            mockDbContext.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            #endregion
        }
        [Fact]
        public async Task StoreBenchMarkResultsAsync_Should_SaveChangesAsync_Once()
        {
            #region Arrange

            var sourceList = new List<BenchMarkResult>();

            var mockDbContext = new Mock<BenchMarkDbContext>();
            var mockedDbSet = new Mock<DbSet<BenchMarkResult>>();
            mockedDbSet.Setup(d => d.Add(It.IsAny<BenchMarkResult>())).Callback<BenchMarkResult>((s) => sourceList.Add(s));
            mockDbContext.Setup(dbContext => dbContext.BenchMarkResult).Returns(mockedDbSet.Object);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                 new BenchMarkResult()
            };

            var testService = new BenchMarkTypeService(null, mockDbContext.Object);

            #endregion

            #region Act

            await testService.StoreBenchMarkResultsAsync(benchMarkResults);

            #endregion

            #region Assert

            mockDbContext.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);


            #endregion
        }
    }
}

