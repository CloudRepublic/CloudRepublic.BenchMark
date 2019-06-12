using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

            _mockBenchMarkResultService.Setup(c => c.GetBenchMarkResults(It.IsAny<int>()))
                .Returns(Task.FromResult((IEnumerable<BenchMarkResult>) benchMarkResults));

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(benchMarkResults))
                .Returns(new BenchMarkData()
                    {CloudProviders = new List<CloudProvider>() {new CloudProvider() {Name = "Azure"}}});


            var trigger = new Trigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);
            var request = TestFactory.CreateHttpRequest();

            var response = await trigger.Run(request, _logger);

            var responseObject = Assert.IsType<OkObjectResult>(response);
            
            
        }
    }
}