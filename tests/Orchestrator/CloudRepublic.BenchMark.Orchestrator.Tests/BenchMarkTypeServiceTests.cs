using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //  Arrange

            var testService = new BenchMarkTypeService(null, null);



            //  Act

            var benchMarkTypes = testService.GetAllTypes();



            //  Assert

            Assert.NotNull(benchMarkTypes);

            // we hard coded verify only the first and last(because its so different) entries for now
            var firstTypeForTesting = benchMarkTypes.First();
            Assert.Equal(CloudProvider.Azure, firstTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Windows, firstTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.FunctionsV4, firstTypeForTesting.Runtime);
            Assert.Equal(Language.Csharp, firstTypeForTesting.Language);
            Assert.Equal("AzureWindowsCsharp", firstTypeForTesting.Name);
            Assert.Equal("AzureWindowsCsharpClient", firstTypeForTesting.ClientName);
            Assert.Equal("AzureWindowsCsharpKey", firstTypeForTesting.KeyName);
            Assert.Equal("AzureWindowsCsharpUrl", firstTypeForTesting.UrlName);
            Assert.True(firstTypeForTesting.SetXFunctionsKey);


            var lastTypeForTesting = benchMarkTypes.Last();
            Assert.Equal(CloudProvider.Firebase, lastTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Linux, lastTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.FunctionsV4, lastTypeForTesting.Runtime);
            Assert.Equal(Language.Nodejs, lastTypeForTesting.Language);
            Assert.Equal("FirebaseLinuxNodejs", lastTypeForTesting.Name);
            Assert.Equal("FirebaseLinuxNodejsClient", lastTypeForTesting.ClientName);
            Assert.Equal("FirebaseLinuxNodejsKey", lastTypeForTesting.KeyName);
            Assert.Equal("FirebaseLinuxNodejsUrl", lastTypeForTesting.UrlName);
            Assert.False(lastTypeForTesting.SetXFunctionsKey);


        }
        [Fact]
        public void GetAllTypes_Should_Return_BenchMarkTypes()
        {
            //  Arrange


            var testService = new BenchMarkTypeService(null, null);



            //  Act

            var benchMarkTypes = testService.GetAllTypes();



            //  Assert

            Assert.NotNull(benchMarkTypes);
            Assert.NotEmpty(benchMarkTypes);


        }


        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Empty_Results_On_No_BenchMarkTypes()
        {
            //  Arrange

            var benchMarks = new List<BenchMarkType>();
            var testService = new BenchMarkTypeService(null, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks);



            //  Assert

            Assert.NotNull(benchMarkResults);
            Assert.Empty(benchMarkResults);


        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_No_Results_For_BenchMarkTypes_when_No_Calls_Are_Asked()
        {
            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needed
                new BenchMarkType()
            };
            var coldCalls = 0;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

            Assert.NotNull(benchMarkResults);
            Assert.Empty(benchMarkResults);



        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Cold_Calls_Are_Asked()
        {
            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 1;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

            Assert.NotNull(benchMarkResults);
            Assert.Single(benchMarkResults);
            Assert.True(benchMarkResults[0].IsColdRequest);



        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Warm_Calls_Are_Asked()
        {
            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 0;
            var warmCalls = 1;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

            Assert.NotNull(benchMarkResults);
            Assert.Single(benchMarkResults);
            Assert.False(benchMarkResults[0].IsColdRequest);



        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_BenchMarkType_when_Cold_And_Warm_Calls_Are_Asked()
        {
            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                // no properties are needer
                new BenchMarkType()
            };
            var coldCalls = 1;
            var warmCalls = 1;

            var benchMarkResponse = new BenchMarkResponse(true, 1);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

            Assert.NotNull(benchMarkResults);
            Assert.Equal(2, benchMarkResults.Count);
            // colds are always run before the warm calls
            Assert.True(benchMarkResults[0].IsColdRequest);
            // warms come after the cold calls
            Assert.False(benchMarkResults[1].IsColdRequest);



        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_Results_For_Multiple_BenchMarkTypes_When_Cold_And_Warm_Calls_Are_Asked()
        {
            //  Arrange

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
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

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


        }
        [Fact]
        public async Task RunBenchMarksAsync_Should_Return_BenchMarkResponse_as_Properly_Converted_Results()
        {

            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                new BenchMarkType(){
                 Name = "TestBenchMark",
                 CloudProvider = CloudProvider.Firebase,
                 HostEnvironment = HostEnvironment.Linux,
                 Runtime = Runtime.FunctionsV4,
                 Language = Language.Fsharp,
                 SetXFunctionsKey = true,
                },
            };
            var coldCalls = 1;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 567);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);



            //  Assert

            Assert.NotNull(benchMarkResults);
            var validationResult = benchMarkResults[0];

            // from the benchmarktype
            Assert.True(validationResult.IsColdRequest);
            Assert.Equal(CloudProvider.Firebase, validationResult.CloudProvider);
            Assert.Equal(HostEnvironment.Linux, validationResult.HostingEnvironment);
            Assert.Equal(Runtime.FunctionsV4, validationResult.Runtime);
            Assert.Equal(Language.Fsharp, validationResult.Language);
            
            // from the actual benchmark
            Assert.Equal(567, validationResult.RequestDuration);
            Assert.True(validationResult.Success);
        }

        [Fact]
        public async Task RunBenchMarksAsync_Should_Call_RunBenchMarkAsync_With_BenchMarkClient_String()
        {

            //  Arrange

            var benchMarks = new List<BenchMarkType>()
            {
                new BenchMarkType(){
                 Name = "TestBenchMark",
                 CloudProvider = CloudProvider.Firebase,
                 HostEnvironment = HostEnvironment.Linux,
                 Runtime = Runtime.FunctionsV4,
                 Language = Language.Fsharp,
                 SetXFunctionsKey = true,
                },
            };
            var coldCalls = 1;
            var warmCalls = 0;

            var benchMarkResponse = new BenchMarkResponse(true, 567);
            _mockIBenchMarkService.Setup(service => service.RunBenchMarkAsync(It.IsAny<string>())).ReturnsAsync(benchMarkResponse);

            var testService = new BenchMarkTypeService(_mockIBenchMarkService.Object, null);



            //  Act

            var benchMarkResults = await testService.RunBenchMarksAsync(benchMarks, coldCalls, warmCalls, 0);


            //  Assert
            _mockIBenchMarkService.Verify(service => service.RunBenchMarkAsync(It.Is<string>(clientName => clientName == "TestBenchMarkClient")), Times.Once);
        }
    }
}

