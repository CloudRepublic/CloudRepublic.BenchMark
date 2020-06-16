using CloudRepublic.BenchMark.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CloudRepublic.BenchMark.SampleFunction.Tests
{
    public class TriggerTests
    {
        private readonly ILogger _logger = MockILoggerFactory.CreateLogger();

        [Fact]
        public async Task Run_Should_Return_BadRequest_When_No_Name_Provided()
        {
            #region Arrange

            var request = MockHttpRequestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await Trigger.Run(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Please pass a name on the query string", responseObject.Value);

            #endregion
        }


        [Theory]
        [InlineData("null")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("helelangenaammaaktmijooknietuitwantditistochmaareenhotencoldtestduswatikhierinvulkanvanalleszijn")]
        [InlineData("{name}")]
        public async Task Run_Should_Return_OkObjectResult_With_Provided_Name_When_Any_Name_given(string nameToTest)
        {
            #region Arrange

            var request = MockHttpRequestFactory.CreateHttpRequest(new Dictionary<string, StringValues>() { { "name", nameToTest } });

            #endregion

            #region Act

            var response = await Trigger.Run(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);
            Assert.Equal($"Hello, {nameToTest}", responseObject.Value);

            #endregion
        }

    }
}