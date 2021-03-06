using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CloudProvider = CloudRepublic.BenchMark.Domain.Enums.CloudProvider;
using Runtime = CloudRepublic.BenchMark.Domain.Enums.Runtime;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class TriggerTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        private readonly Mock<IBenchMarkResultService> _mockBenchMarkResultService;

        private Mock<IResponseConverterService> _mockResponseConverter;

        public TriggerTests()
        {
            _mockBenchMarkResultService = new Mock<IBenchMarkResultService>();
            _mockResponseConverter = new Mock<IResponseConverterService>();
        }

        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_CloudProvider_argument_given()
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"hostingEnvironment", "Windows"},
                {"runtime", "Csharp"}
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }
        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_hostingEnvironment_argument_given()
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"runtime", "Csharp"}
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }
        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_runtime_argument_given()
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"hostingEnvironment", "Windows"},
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }

        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID CLOUDPROVIDER VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task Run_Should_Return_BadRequest_When_Invalid_CloudProvider_argument_given(string argumentValue)
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", argumentValue},
                {"hostingEnvironment", "Windows"},
                {"runtime", "Csharp"},
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }


        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID HOSTINGENVIRONMENT VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task Run_Should_Return_BadRequest_When_Invalid_HostingEnvironment_argument_given(string argumentValue)
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"hostingEnvironment", argumentValue},
                {"runtime", "Csharp"},
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }


        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID RUNTIME VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task Run_Should_Return_BadRequest_When_Invalid_runtime_argument_given(string argumentValue)
        {
            //  Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"hostingEnvironment", "Windows"},
                {"runtime", argumentValue},
            });



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<BadRequestResult>(response);


        }

        [Fact]
        public async Task Run_Should_Call_BenchMarkResultService_With_QueryParameters()
        {
            //  Arrange

            var benchMarkResults = new List<BenchMarkResult>();

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));
            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "Azure" };

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(benchMarkResults))
                .Returns(sampleBenchMarkData);

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            _mockBenchMarkResultService.Verify(service => service.GetBenchMarkResultsAsync(
                It.Is<CloudProvider>(cloudProvider => cloudProvider == CloudProvider.Firebase),
                It.Is<HostEnvironment>(hostingEnvironment => hostingEnvironment == HostEnvironment.Linux),
                It.Is<Runtime>(runtime => runtime == Runtime.Fsharp),
                It.IsAny<DateTime>()), Times.Once);


        }
        [Fact]
        public async Task Run_Should_Call_BenchMarkResultService_For_DateTimeNow()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));


            var benchMarkResults = new List<BenchMarkResult>();

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));


            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            _mockBenchMarkResultService.Verify(service => service.GetDateTimeNow(), Times.Once);


        }
        [Fact]
        public async Task Run_Should_Call_BenchMarkResultService_With_DateNow_From_Service_Combined_With_Environment_DateRange()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "10");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 21, 1, 3, 44));

            var benchMarkResults = new List<BenchMarkResult>();

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));


            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            _mockBenchMarkResultService.Verify(service => service.GetBenchMarkResultsAsync(
                It.IsAny<CloudProvider>(),
                It.IsAny<HostEnvironment>(),
                It.IsAny<Runtime>(),
                It.Is<DateTime>(sinceDate => sinceDate == new DateTime(2020, 1, 11, 1, 3, 44))), Times.Once);
            // the given day was 21, minus the daterange 10 it should become the exact same date but 10 days earlier


        }


        [Fact]
        public async Task Run_Should_Return_NotFoundResult_When_Service_returns_No_BenchMarkData()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));

            var benchMarkResults = new List<BenchMarkResult>();

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<NotFoundResult>(response);


        }
        [Fact]
        public async Task Run_Should_Return_NotFoundResult_When_Service_returns_No_Succes_BenchMarkData()
        {
            //  Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Success = false,
                },
                new BenchMarkResult()
                {
                    Success = false,
                }
            };

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));
            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            Assert.IsType<NotFoundResult>(response);


        }


        [Fact]
        public async Task Run_Should_Call_ResponseConverter_With_Result_Of_BenchmarkResultService()
        {
            //  Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Id = 77,
                    Success = true,
                }
            };

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));
            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "ReturnedData" };

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            _mockResponseConverter.Verify(service => service.ConvertToBenchMarkData(
                It.Is<List<BenchMarkResult>>(results => results[0].Id == benchMarkResults[0].Id)), Times.Once);


        }
        [Fact]
        public async Task Run_Should_Return_OkObjectResult_Containing_ConvertedBenchMarkData_From_Service()
        {
            //  Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Success = true,
                }
            };

            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetDateTimeNow()).Returns(new DateTime(2020, 1, 2));
            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Runtime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "ReturnedData" };

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(benchMarkResults))
                .Returns(sampleBenchMarkData);

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Firebase"}, {"hostingEnvironment", "Linux"}, {"runtime", "Fsharp"}});



            //  Act

            var response = await trigger.Run(request, _logger);



            //  Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);

            var benchMarkData = Assert.IsType<BenchMarkData>(responseObject.Value);

            Assert.Equal(sampleBenchMarkData.CloudProvider, benchMarkData.CloudProvider);


        }

    }
}