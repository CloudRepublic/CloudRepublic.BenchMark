using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CloudRepublic.BenchMark.Orchestrator.Tests
{
    public class BenchMarkOrchestratorTests
    {

        private readonly Mock<IBenchMarkTypeService> _mockIBenchMarkTypeService;

        public BenchMarkOrchestratorTests()
        {
            _mockIBenchMarkTypeService = new Mock<IBenchMarkTypeService>();
        }
        [Fact]
        public async Task RunAndHandleAllBenchMarksAsync_Should_Call_BenchMarkTypeService_for_All_Types()
        {
            //  Arrange


            var benchMarkTypes = new List<BenchMarkType>();
            _mockIBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(benchMarkTypes);

            var benchMarkResults = new List<BenchMarkResult>();
            _mockIBenchMarkTypeService.Setup(service => service.RunBenchMarksAsync(It.IsAny<List<BenchMarkType>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(benchMarkResults);

            var benchMarkOrchestrator = new BenchMarkOrchestrator(_mockIBenchMarkTypeService.Object);




            //  Act

            await benchMarkOrchestrator.RunAndHandleAllBenchMarksAsync();



            //  Assert

            _mockIBenchMarkTypeService.Verify(service => service.GetAllTypes(), Times.Once);


        }
        [Fact]
        public async Task RunAndHandleAllBenchMarksAsync_Should_Call_BenchMarkTypeService_RunBenchMarks_With_All_Types()
        {
            //  Arrange
            var benchMarkTypes = new List<BenchMarkType>()
            {
                 new BenchMarkType()
                 {
                      Name = "Henkie"
                 }
            };
            _mockIBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(benchMarkTypes);

            var benchMarkResults = new List<BenchMarkResult>();
            _mockIBenchMarkTypeService.Setup(service => service.RunBenchMarksAsync(It.IsAny<List<BenchMarkType>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(benchMarkResults);

            var benchMarkOrchestrator = new BenchMarkOrchestrator(_mockIBenchMarkTypeService.Object);



            //  Act

            await benchMarkOrchestrator.RunAndHandleAllBenchMarksAsync();



            //  Assert

            _mockIBenchMarkTypeService.Verify(service => service.RunBenchMarksAsync(It.Is<List<BenchMarkType>>(benchMarks => benchMarks[0].Name == "Henkie"), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);


        }
        [Fact]
        public async Task RunAndHandleAllBenchMarksAsync_Should_Call_BenchMarkTypeService_StoreBenchMarks_With_Results_from_RunBenchMarks()
        {
            //  Arrange
            var benchMarkTypes = new List<BenchMarkType>();
            _mockIBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(benchMarkTypes);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                     Id = "800",
                }
            };
            _mockIBenchMarkTypeService.Setup(service => service.RunBenchMarksAsync(It.IsAny<List<BenchMarkType>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(benchMarkResults);

            var benchMarkOrchestrator = new BenchMarkOrchestrator(_mockIBenchMarkTypeService.Object);



            //  Act

            await benchMarkOrchestrator.RunAndHandleAllBenchMarksAsync();



            //  Assert

            _mockIBenchMarkTypeService.Verify(service => service.StoreBenchMarkResultsAsync(It.Is<List<BenchMarkResult>>(results => results[0].Id == "800")), Times.Once);


        }
        [Fact]
        public async Task RunAndHandleAllBenchMarksAsync_Should_Not_Call_BenchMarkTypeService_StoreBenchMarks_Without_Results_from_RunBenchMarks()
        {
            //  Arrange
            var benchMarkTypes = new List<BenchMarkType>();
            _mockIBenchMarkTypeService.Setup(service => service.GetAllTypes()).Returns(benchMarkTypes);

            var benchMarkResults = new List<BenchMarkResult>();
            _mockIBenchMarkTypeService.Setup(service => service.RunBenchMarksAsync(It.IsAny<List<BenchMarkType>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(benchMarkResults);

            var benchMarkOrchestrator = new BenchMarkOrchestrator(_mockIBenchMarkTypeService.Object);



            //  Act

            await benchMarkOrchestrator.RunAndHandleAllBenchMarksAsync();



            //  Assert

            _mockIBenchMarkTypeService.Verify(service => service.StoreBenchMarkResultsAsync(It.IsAny<List<BenchMarkResult>>()), Times.Never);


        }
    }
}
