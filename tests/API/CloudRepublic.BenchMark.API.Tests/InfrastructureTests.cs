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
                    CloudProvider = (int)cloudProvider,
                    HostingEnvironment = (int)hostingEnvironment,
                    Runtime = (int)runtime,
                    Success = true,
                    CreatedAt = DateTimeOffset.Now,
                    RequestDuration = 200
                },
                new BenchMarkResult()
                {
                    Id = 0,
                    CloudProvider = (int)cloudProvider,
                    HostingEnvironment = (int)hostingEnvironment,
                    Runtime = (int)runtime,
                    Success = true,
                    CreatedAt = DateTimeOffset.Now,
                    RequestDuration = 200
                }
            };

            var responseConverter = new ResponseConverter();
            #endregion

            #region Act
            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);
            #endregion

            #region Assert
            Assert.NotNull(benchMarkData);
            Assert.True(benchMarkData.CloudProviders.Any());
            Assert.True(benchMarkData.CloudProviders.First().Name == cloudProvider.ToString());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.Any());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Name == hostingEnvironment.ToString());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Runtimes.Any());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Runtimes.First().Name ==
                        runtime.ToString());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Runtimes.First()
                            .AverageExecutionTime == benchMarkResults.First().RequestDuration);
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Runtimes.First().DataPoints
                .Any());
            Assert.True(benchMarkData.CloudProviders.First().HostingEnvironments.First().Runtimes.First().DataPoints
                            .First().ExecutionTime == benchMarkResults.First().RequestDuration);
            #endregion
            
        }
    }
}