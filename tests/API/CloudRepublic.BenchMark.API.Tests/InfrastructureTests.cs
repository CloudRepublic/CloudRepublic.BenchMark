using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class InfrastructureTests
    {
        [Fact]
        public void ResponseConverterShouldConvertBenchMarkResultIntoBenchMarkData()
        {
            #region Arrange

            var cloudProvider = CloudProvider.Azure;
            var hostingEnvironment = HostEnvironment.Windows;
            var runtime = Runtime.Csharp;


            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    Id = 0,
                    CloudProvider = (int) cloudProvider,
                    HostingEnvironment = (int) hostingEnvironment,
                    Runtime = (int) runtime,
                    Success = true,
                    CreatedAt = DateTimeOffset.Now,
                    RequestDuration = 200,
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    Id = 0,
                    CloudProvider = (int) cloudProvider,
                    HostingEnvironment = (int) hostingEnvironment,
                    Runtime = (int) runtime,
                    Success = true,
                    CreatedAt = DateTimeOffset.Now,
                    RequestDuration = 200,
                    IsColdRequest = true
                }
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.True(benchMarkData.ColdDataPoints.Count == 2);
            Assert.True(benchMarkData.HotDataPoints.Count == 0);
            Assert.True(benchMarkData.CloudProvider == cloudProvider.ToString());
            Assert.True(benchMarkData.HostingEnvironment == hostingEnvironment.ToString());
            Assert.True(benchMarkData.Runtime == runtime.ToString());

            #endregion
        }
    }
}