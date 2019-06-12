using System.Threading.Tasks;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CloudRepublic.BenchMark.SampleFunction.Tests
{
    public class FunctionTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        
        [Fact]
        public async Task FunctionShouldReturnProvidedName()
        {
            string nameValue = "BenchCloud";

            var request = TestFactory.CreateHttpRequest("name", nameValue);
            var response = await Trigger.Run(request, _logger);

            var responseObject = Assert.IsType<OkObjectResult>(response);
            Assert.Equal($"Hello, {nameValue}", responseObject.Value);
        }

        [Fact]
        public async Task FunctionShouldReturnBadRequest()
        {
            var request = TestFactory.CreateHttpRequest();

            var response = await Trigger.Run(request, _logger);

            var responseObject = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Please pass a name on the query string", responseObject.Value);
        }
    }
}
