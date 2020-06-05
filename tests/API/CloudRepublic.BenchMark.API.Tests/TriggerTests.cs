using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class TriggerTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        private readonly Mock<IBenchMarkResultService> _mockBenchMarkResultService;

        private Mock<IResponseConverter> _mockResponseConverter;

        public TriggerTests()
        {
            _mockBenchMarkResultService = new Mock<IBenchMarkResultService>();
            _mockResponseConverter = new Mock<IResponseConverter>();
        }

        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_CloudProvider_argument_given()
        {
            #region Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"hostingEnvironment", "Windows"},
                {"runtime", "Csharp"}
            });

            #endregion

            #region Act

            var response = await trigger.Run(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }
        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_hostingEnvironment_argument_given()
        {
            #region Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"runtime", "Csharp"}
            });

            #endregion

            #region Act

            var response = await trigger.Run(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }

        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_runtime_argument_given()
        {
            #region Arrange

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()                {
                {"cloudProvider", "Azure"},
                {"hostingEnvironment", "Windows"},
            });

            #endregion

            #region Act

            var response = await trigger.Run(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }
        [Fact]
        public async Task Run_Should_Return_OkObjectResult_Containing_BenchMarkData_When________________()
        {
            #region Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Id = 1,
                    CloudProvider = 0,
                    HostingEnvironment = 0,
                    Runtime = 0,
                    CreatedAt = DateTimeOffset.Now,
                    RequestDuration = 200,
                    Success = true
                }
            };

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResults(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "Azure" };

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(benchMarkResults))
                .Returns(sampleBenchMarkData);

            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>()
                {{"cloudProvider", "Azure"}, {"hostingEnvironment", "Windows"}, {"runtime", "Csharp"}});

            #endregion

            #region Act

            var response = await trigger.Run(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);

            var benchMarkData = Assert.IsType<BenchMarkData>(responseObject.Value);

            Assert.Equal(sampleBenchMarkData.CloudProvider, benchMarkData.CloudProvider);

            #endregion
        }
    }
}