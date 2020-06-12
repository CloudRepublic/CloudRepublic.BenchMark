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
using Language = CloudRepublic.BenchMark.Domain.Enums.Language;

namespace CloudRepublic.BenchMark.API.Tests
{
    public static class DictionaryExtensionForTesting
    {
        public static void SetDefaultHeaders_All(this Dictionary<string, StringValues> dictionary)
        {
            dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();
        }
        public static void SetDefaultHeader_CloudProvider(this Dictionary<string, StringValues> dictionary, string cloudProvider = "Azure")
        {
            dictionary.Add("cloudProvider", cloudProvider);
        }
        public static void SetDefaultHeader_Language(this Dictionary<string, StringValues> dictionary, string language = "Csharp")
        {
            dictionary.Add("language", language);
        }
        public static void SetDefaultHeader_AzureRuntimeVersion(this Dictionary<string, StringValues> dictionary, string azureRuntimeVersion = "Version_2")
        {
            dictionary.Add("azureRuntimeVersion", azureRuntimeVersion);
        }
        public static void SetDefaultHeader_HostingEnvironment(this Dictionary<string, StringValues> dictionary, string hostingEnvironment = "Windows")
        {
            dictionary.Add("hostingEnvironment", hostingEnvironment);
        }
    }
    public class GetResultsTriggerTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        private readonly Mock<IBenchMarkResultService> _mockBenchMarkResultService;

        private Mock<IResponseConverterService> _mockResponseConverter;

        public GetResultsTriggerTests()
        {
            _mockBenchMarkResultService = new Mock<IBenchMarkResultService>();
            _mockResponseConverter = new Mock<IResponseConverterService>();

            // default dayrange and datetime
            Environment.SetEnvironmentVariable("dayRange", "1");
            _mockBenchMarkResultService.Setup(c => c.GetToday()).Returns(new DateTime(2020, 1, 2));

            // setup a default empty return
            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Language>(), It.IsAny<AzureRuntimeVersion>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new List<BenchMarkResult>()));
        }

        [Fact]
        public async Task GetResultsAsync_Should_Return_BadRequest_When_No_CloudProvider_argument_given()
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            // dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }

        [Fact]
        public async Task GetResultsAsync_Should_Return_BadRequest_When_No_AzureRuntimeVersion_argument_given()
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language();
            // dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Return_BadRequest_When_No_hostingEnvironment_argument_given()
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            // dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Return_BadRequest_When_No_language_argument_given()
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider();
            // dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }

        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID CLOUDPROVIDER VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task GetResultsAsync_Should_Return_BadRequest_When_Invalid_CloudProvider_argument_given(string argumentValue)
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider(argumentValue);
            dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }


        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID HOSTINGENVIRONMENT VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task GetResultsAsync_Should_Return_BadRequest_When_Invalid_HostingEnvironment_argument_given(string argumentValue)
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language();
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment(argumentValue);

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }


        [Theory]
        [InlineData(null)] // parameter is given, but no value
        [InlineData("")] // parameter is given, but no value
        [InlineData(" ")] // parameter is given, but whitespace value
        [InlineData("        ")]// parameter is given, but whitespace value
        [InlineData("INVALID LANGUAGE VALUE")]// parameter is given, but invalid name value
        [InlineData("-1")] // parameter is given, but out of range number value
        [InlineData("17")] // parameter is given, but out of range number value
        public async Task GetResultsAsync_Should_Return_BadRequest_When_Invalid_language_argument_given(string argumentValue)
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider();
            dictionary.SetDefaultHeader_Language(argumentValue);
            dictionary.SetDefaultHeader_AzureRuntimeVersion();
            dictionary.SetDefaultHeader_HostingEnvironment();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<BadRequestResult>(response);

            #endregion
        }

        [Fact]
        public async Task GetResultsAsync_Should_Call_BenchMarkResultService_With_QueryParameters()
        {
            #region Arrange

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "Azure" };

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(new List<BenchMarkResult>())).Returns(sampleBenchMarkData);

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeader_CloudProvider(CloudProvider.Firebase.ToString());
            dictionary.SetDefaultHeader_HostingEnvironment(HostEnvironment.Linux.ToString());
            dictionary.SetDefaultHeader_Language(Language.Fsharp.ToString());
            dictionary.SetDefaultHeader_AzureRuntimeVersion(AzureRuntimeVersion.Version_3.ToString());

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            _mockBenchMarkResultService.Verify(service => service.GetBenchMarkResultsAsync(
                It.Is<CloudProvider>(cloudProvider => cloudProvider == CloudProvider.Firebase),
                It.Is<HostEnvironment>(hostingEnvironment => hostingEnvironment == HostEnvironment.Linux),
                It.Is<Language>(language => language == Language.Fsharp),
                It.Is<AzureRuntimeVersion>(azureRuntimeVersion => azureRuntimeVersion == AzureRuntimeVersion.Version_3),
                It.IsAny<DateTime>()), Times.Once);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Call_BenchMarkResultService_For_Todays_Date()
        {
            #region Arrange

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            _mockBenchMarkResultService.Verify(service => service.GetToday(), Times.Once);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Call_BenchMarkResultService_With_Date_Of_Today_Combined_With_Environment_DateRange()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "10");
            _mockBenchMarkResultService.Setup(c => c.GetToday()).Returns(new DateTime(2020, 1, 21, 1, 3, 44));


            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            _mockBenchMarkResultService.Verify(service => service.GetBenchMarkResultsAsync(
                It.IsAny<CloudProvider>(),
                It.IsAny<HostEnvironment>(),
                It.IsAny<Language>(),
                It.IsAny<AzureRuntimeVersion>(),
                It.Is<DateTime>(sinceDate => sinceDate == new DateTime(2020, 1, 11, 1, 3, 44))), Times.Once);
            // the given day was 21, minus the daterange 10 it should become the exact same date but 10 days earlier

            #endregion
        }


        [Fact]
        public async Task GetResultsAsync_Should_Return_NotFoundResult_When_Service_returns_No_BenchMarkData()
        {
            #region Arrange


            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<NotFoundResult>(response);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Return_NotFoundResult_When_Service_returns_No_Succes_BenchMarkData()
        {
            #region Arrange

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

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Language>(), It.IsAny<AzureRuntimeVersion>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            Assert.IsType<NotFoundResult>(response);

            #endregion
        }


        [Fact]
        public async Task GetResultsAsync_Should_Call_ResponseConverter_With_Result_Of_BenchmarkResultService()
        {
            #region Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Id = 77,
                    Success = true,
                }
            };

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Language>(), It.IsAny<AzureRuntimeVersion>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "ReturnedData" };

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            _mockResponseConverter.Verify(service => service.ConvertToBenchMarkData(
                It.Is<List<BenchMarkResult>>(results => results[0].Id == benchMarkResults[0].Id)), Times.Once);

            #endregion
        }
        [Fact]
        public async Task GetResultsAsync_Should_Return_OkObjectResult_Containing_ConvertedBenchMarkData_From_Service()
        {
            #region Arrange

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Success = true,
                }
            };

            _mockBenchMarkResultService.Setup(c =>
                    c.GetBenchMarkResultsAsync(It.IsAny<CloudProvider>(), It.IsAny<HostEnvironment>(), It.IsAny<Language>(), It.IsAny<AzureRuntimeVersion>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(benchMarkResults));

            var sampleBenchMarkData = new BenchMarkData()
            { CloudProvider = "ReturnedData" };

            _mockResponseConverter.Setup(c => c.ConvertToBenchMarkData(benchMarkResults))
                .Returns(sampleBenchMarkData);

            var trigger = new GetResultsTrigger(_mockBenchMarkResultService.Object, _mockResponseConverter.Object);

            var dictionary = new Dictionary<string, StringValues>();
            dictionary.SetDefaultHeaders_All();

            var request = TestFactory.CreateHttpRequest(dictionary);

            #endregion

            #region Act

            var response = await trigger.GetResultsAsync(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);

            var benchMarkData = Assert.IsType<BenchMarkData>(responseObject.Value);

            Assert.Equal(sampleBenchMarkData.CloudProvider, benchMarkData.CloudProvider);

            #endregion
        }

    }
}