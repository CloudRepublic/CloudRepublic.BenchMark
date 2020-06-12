using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class GetBenchMarkOptionsTriggerTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();
        private readonly Mock<IBenchMarkTypeService> _mockBenchMarkTypeService;
        private Mock<IResponseConverterService> _mockResponseConverter;

        public GetBenchMarkOptionsTriggerTests()
        {
            _mockBenchMarkTypeService = new Mock<IBenchMarkTypeService>();
            _mockResponseConverter = new Mock<IResponseConverterService>();
        }


        [Fact]
        public async Task GetResultsAsync_Should_Call_BenchMarkTypeService_For_BenchMark_Types()
        {
            #region Arrange

            _mockBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(new List<BenchMarkType>());

            var trigger = new GetBenchMarkOptionsTrigger(_mockBenchMarkTypeService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await trigger.GetBenchMarkTypesAsync(request, _logger);

            #endregion

            #region Assert

            _mockBenchMarkTypeService.Verify(service => service.GetAllTypes(), Times.Once);

            #endregion
        }
        [Fact]
        public async Task GetBenchMarkTypesAsync_Should_Return_OkObjectResult_with_Empty_List_On_No_BenchMarkTypes_From_BenchMarkTypeService()
        {
            #region Arrange

            _mockBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(new List<BenchMarkType>());
            _mockResponseConverter.Setup(service => service.ConvertToBenchMarkOptions(It.IsAny<List<BenchMarkType>>())).Returns(new List<BenchMarkOption>());

            var trigger = new GetBenchMarkOptionsTrigger(_mockBenchMarkTypeService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await trigger.GetBenchMarkTypesAsync(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);
            var benchMarkData = Assert.IsType<List<BenchMarkOption>>(responseObject.Value);
            Assert.Empty(benchMarkData);

            #endregion
        }
        [Fact]
        public async Task GetBenchMarkTypesAsync_Should_Return_Call_ResultConverter_with_BenchMarkTypes_From_BenchMarkTypeService()
        {
            #region Arrange

            _mockBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(new List<BenchMarkType>()
            {
                new BenchMarkType()
                {
                     Name ="Henk"
                }
            });


            var trigger = new GetBenchMarkOptionsTrigger(_mockBenchMarkTypeService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await trigger.GetBenchMarkTypesAsync(request, _logger);

            #endregion

            #region Assert

            _mockResponseConverter.Verify(service => service.ConvertToBenchMarkOptions(It.Is<List<BenchMarkType>>(list => list[0].Name == "Henk")), Times.Once);


            #endregion
        }

        [Fact]
        public async Task GetBenchMarkTypesAsync_Should_Return_OkObject_With_BenchMarkTypes_From_BenchMarkTypeService()
        {
            #region Arrange

            _mockResponseConverter.Setup(service => service.ConvertToBenchMarkOptions(It.IsAny<List<BenchMarkType>>())).Returns(new List<BenchMarkOption>()
            {
                 new BenchMarkOption()
                 {
                      CloudProviderName = "Cloud",
                      AzureRuntimeVersionName = "Version",
                      LanguageName = "Language",
                      HostEnvironmentName = "Environment",
                 }
            });


            var trigger = new GetBenchMarkOptionsTrigger(_mockBenchMarkTypeService.Object, _mockResponseConverter.Object);

            var request = TestFactory.CreateHttpRequest();

            #endregion

            #region Act

            var response = await trigger.GetBenchMarkTypesAsync(request, _logger);

            #endregion

            #region Assert

            var responseObject = Assert.IsType<OkObjectResult>(response);

            var benchMarkData = Assert.IsType<List<BenchMarkOption>>(responseObject.Value);

            // dit test indirect ook dat de title goed gezet wordt. deze kun je gewoon aanpassen als je frontend een andere titel wil
            Assert.Equal("Cloud Environment Language Version", benchMarkData[0].Title);

            #endregion
        }

    }
}