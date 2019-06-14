using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace CloudRepublic.BenchMark.SampleFunction.Tests
{
    public class FunctionTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();

        [Fact]
        public async Task FunctionShouldReturnProvidedName()
        {
            #region Arrange

            string nameValue = "BenchCloud";

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>() {{"name", nameValue}});

            #endregion

            #region Act

            var response = await Trigger.Run(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);
            Assert.Equal($"Hello, {nameValue}", responseObject.Value);

            #endregion
        }

        [Fact]
        public async Task FunctionShouldReturnBadRequest()
        {
            #region Arrange

            var request = TestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await Trigger.Run(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Please pass a name on the query string", responseObject.Value);

            #endregion
        }
    }
}