using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class FunctionTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        private Mock<IBenchMarkResultService> _mockBenchMarkResultService;

        private Mock<IResponseConverter> _mockResponseConverter;

        public FunctionTests()
        {
            _mockBenchMarkResultService = new Mock<IBenchMarkResultService>();
            _mockResponseConverter = new Mock<IResponseConverter>();
        }

        [Fact]
        public async Task FunctionShouldReturnOkObjectResultContainingBenchMarkData()
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
                {CloudProvider = "Azure"};

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

            Assert.Equal(benchMarkData.CloudProvider, sampleBenchMarkData.CloudProvider);

            #endregion
        }
    }
}