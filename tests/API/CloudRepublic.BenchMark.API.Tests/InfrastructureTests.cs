using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class InfrastructureTests
    {
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_RequestData_Into_BenchMarkData()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");


            var cloudProvider = CloudProvider.Azure;
            var hostingEnvironment = HostEnvironment.Windows;
            var runtime = Runtime.Csharp;


            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CloudProvider = (int) cloudProvider,
                    HostingEnvironment = (int) hostingEnvironment,
                    Runtime = (int) runtime,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(cloudProvider.ToString(), benchMarkData.CloudProvider);
            Assert.Equal(hostingEnvironment.ToString(), benchMarkData.HostingEnvironment);
            Assert.Equal(runtime.ToString(), benchMarkData.Runtime);

            #endregion
        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_RequestData_Of_First_Result_Into_BenchMarkData_When_Conflicting_RequestData()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CloudProvider =  (int)CloudProvider.Firebase,
                    HostingEnvironment = (int) HostEnvironment.Linux,
                    Runtime = (int) Runtime.Java,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new BenchMarkResult()
                {
                    CloudProvider = (int)CloudProvider.Azure, // different than the main
                    HostingEnvironment = (int) HostEnvironment.Windows,
                    Runtime = (int) Runtime.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new BenchMarkResult()
                {
                    CloudProvider = (int)CloudProvider.Azure, // different than the main
                    HostingEnvironment = (int) HostEnvironment.Windows,
                    Runtime = (int) Runtime.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new BenchMarkResult()
                {
                    CloudProvider = (int)CloudProvider.Azure, // different than the main
                    HostingEnvironment = (int) HostEnvironment.Windows,
                    Runtime = (int) Runtime.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(CloudProvider.Firebase.ToString(), benchMarkData.CloudProvider);
            Assert.Equal(HostEnvironment.Linux.ToString(), benchMarkData.HostingEnvironment);
            Assert.Equal(Runtime.Java.ToString(), benchMarkData.Runtime);

            #endregion
        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_Enum_Number_Into_BenchMarkData_When_RequestData_Is_Outside_Of_EnumRange()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CloudProvider =  -1, // does not exist on enum
                    HostingEnvironment =100, // does not exist on enum
                    Runtime = 789, // does not exist on enum
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal("-1", benchMarkData.CloudProvider);
            Assert.Equal("100", benchMarkData.HostingEnvironment);
            Assert.Equal("789", benchMarkData.Runtime);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Throws_InvalidOperationException_when_No_Results_Given()
        {
            #region Arrange

            var benchMarkResults = new List<BenchMarkResult>() { };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act & Assert

            Assert.Throws<InvalidOperationException>(() => responseConverter.ConvertToBenchMarkData(benchMarkResults));

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Convert_BenchMarkResult_Date_With_The_Enviroment_DataRange_Into_DataPoints_Regardless_Of_Cold_Or_Warm_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "20");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)), // we need a date and as such a singular result
                    IsColdRequest = true// even though this is a cold request warm data points are still added
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            // 20 of the date range
            Assert.Equal(20, benchMarkData.ColdDataPoints.Count);
            Assert.Equal(20, benchMarkData.WarmDataPoints.Count);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Cold_Or_Warm_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(4, benchMarkData.ColdDataPoints[0].ExecutionTimes.Count);
            Assert.Single(benchMarkData.WarmDataPoints[0].ExecutionTimes);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Cold_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(4, benchMarkData.ColdDataPoints[0].ExecutionTimes.Count);
            Assert.Empty(benchMarkData.WarmDataPoints[0].ExecutionTimes);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Warm_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");



            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Empty(benchMarkData.ColdDataPoints[0].ExecutionTimes);
            Assert.Equal(4, benchMarkData.WarmDataPoints[0].ExecutionTimes.Count);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Calculate_Correct_Medians_By_Warm_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");



            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 120,
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 160,
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)), //today
                    RequestDuration = 84,
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 42,
                    IsColdRequest = false
                },
                // cold request that can be ignored and should not be taken into the medians
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 1220,
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 8160,
                    IsColdRequest = true
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(63, benchMarkData.WarmMedianExecutionTime); // 42-84 = 63 median
            Assert.Equal(-122.22, benchMarkData.WarmPreviousDayDifference);// (42-84)(63) vs (120-160)(140) = -77 diff = -122.22%
            Assert.True(benchMarkData.WarmPreviousDayPositive);

            #endregion
        }
        [Fact]
        public void ResponseConverter_Should_Calculate_Correct_Medians_By_Cold_Status()
        {
            #region Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 120,
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 160,
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)), //today
                    RequestDuration = 84,
                    IsColdRequest = true
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 42,
                    IsColdRequest = true
                },

                // warm request that can be ignored and should not be taken into the medians
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)), //yesterday
                    RequestDuration = 7684,
                    IsColdRequest = false
                },
                new BenchMarkResult()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 9942,
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverter();

            #endregion

            #region Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(63, benchMarkData.ColdMedianExecutionTime); // 42-84 = 63 median
            Assert.Equal(-122.22, benchMarkData.ColdPreviousDayDifference);// (42-84)(63) vs (120-160)(140) = -77 diff = -122.22%
            Assert.True(benchMarkData.ColdPreviousDayPositive);

            #endregion
        }
    }
}