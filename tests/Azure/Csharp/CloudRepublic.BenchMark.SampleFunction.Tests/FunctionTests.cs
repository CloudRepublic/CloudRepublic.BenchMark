using CloudRepublic.BenchMark.SampleFunction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BenchCloud.CSharp.Tests
{
    public class FunctionTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
        
        [Fact]
        public async Task FunctionShouldReturnProvidedName()
        {
            string nameValue = "BenchCloud";

            var request = TestFactory.CreateHttpRequest("name", nameValue);
            var response = await Trigger.Run(request, logger);

            var responseObject = Assert.IsType<OkObjectResult>(response);
            Assert.Equal($"Hello, {nameValue}", responseObject.Value);
        }

        [Fact]
        public async Task FunctionShouldReturnBadRequest()
        {
            var request = TestFactory.CreateInvalidHttpRequest();

            var response = await Trigger.Run(request, logger);

            var responseObject = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Please pass a name on the query string", responseObject.Value);
        }
    }
}
