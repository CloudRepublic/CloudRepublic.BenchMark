using CloudRepublic.BenchMark.Application.Services;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CloudRepublic.BenchMark.Application.Tests
{
    public class BenchMarkServiceTests
    {
        [Theory]
        [InlineData("")] // what the name is doesnt matter
        [InlineData("  ")]
        [InlineData("x")]
        [InlineData("null")]
        [InlineData("-1")]
        public async Task RunBenchMarkAsync_Should_Call_HttpClientFactory_With_Given_Name(string givenName)
        {
            //  Arrange

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var results = await testService.RunBenchMarkAsync(givenName);



            //  Assert

            httpClientFactoryMoq.Verify(factory => factory.CreateClient(It.Is<string>(clientName => clientName == givenName)), Times.Once);


        }

        [Fact]
        public async Task RunBenchMarkAsync_Should_Catch_Exception_Of_HttpClient_and_Return_Failed_Response()
        {

            //  Arrange

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ThrowsAsync(new HttpRequestException("test failure"));

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://validURI.com/"),
            };

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();
            httpClientFactoryMoq.Setup(factory => factory.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var result = await testService.RunBenchMarkAsync("ClientName");



            //  Assert

            Assert.False(result.Success);
            Assert.Equal(0, result.Duration);



        }

        [Fact]
        public async Task RunBenchMarkAsync_Should_Call_HttpClient_By_Given_Name_And_BenchMark_Trigger()
        {

            //  Arrange

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://validURI.com/"),
            };

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();
            httpClientFactoryMoq.Setup(factory => factory.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var result = await testService.RunBenchMarkAsync("ClientName");



            //  Assert

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri("http://validURI.com/api/Trigger?name=BenchMark");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // we expected a GET request
                  && req.RequestUri == expectedUri // to this uri
               ),
               ItExpr.IsAny<CancellationToken>()
            );


        }

        [Fact]
        public async Task RunBenchMarkAsync_Should_Return_A_Failed_Response_With_Duration_When_HttpClient_Returns_A_Non_Succes_StatusCode_After_A_Delay()
        {

            //  Arrange

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.ServiceUnavailable,
               }, new TimeSpan(0, 0, 1))
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://validURI.com/"),
            };

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();
            httpClientFactoryMoq.Setup(factory => factory.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var result = await testService.RunBenchMarkAsync("ClientName");



            //  Assert

            Assert.False(result.Success);
            Assert.NotEqual(0, result.Duration);



        }

        [Fact]
        public async Task RunBenchMarkAsync_Should_Return_An_Succesfull_Response_When_HttpClient_Returns_Succes()
        {

            //  Arrange

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://validURI.com/"),
            };

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();
            httpClientFactoryMoq.Setup(factory => factory.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var result = await testService.RunBenchMarkAsync("ClientName");



            //  Assert

            Assert.True(result.Success);



        }

        [Fact]
        public async Task RunBenchMarkAsync_Should_Return_A_Duration_When_HttpClient_Returns_Succes_After_A_Delay()
        {
            //  Arrange

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               }, new TimeSpan(0, 0, 1))
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://validURI.com/"),
            };

            var httpClientFactoryMoq = new Mock<IHttpClientFactory>();
            httpClientFactoryMoq.Setup(factory => factory.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var testService = new BenchMarkService(httpClientFactoryMoq.Object);



            //  Act

            var result = await testService.RunBenchMarkAsync("ClientName");



            //  Assert

            Assert.True(result.Success);
            Assert.NotEqual(0, result.Duration);



        }

    }
}

