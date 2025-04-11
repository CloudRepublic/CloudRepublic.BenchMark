using CloudRepublic.BenchMark.API.V2.Services;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.Data;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class ResponseConverterTests
    {
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_RequestData_Into_BenchMarkData()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");


            var cloudProvider = CloudProvider.Azure;
            var hostingEnvironment = HostEnvironment.Windows;
            var runtime = Runtime.FunctionsV4;


            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CloudProvider = cloudProvider,
                    HostingEnvironment = hostingEnvironment,
                    Runtime = runtime,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(cloudProvider.ToString(), benchMarkData.CloudProvider);
            Assert.Equal(hostingEnvironment.ToString(), benchMarkData.HostingEnvironment);
            Assert.Equal(runtime.GetName(), benchMarkData.Runtime);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_RequestData_Of_First_Result_Into_BenchMarkData_When_Conflicting_RequestData()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CloudProvider =  CloudProvider.Firebase,
                    HostingEnvironment =  HostEnvironment.Linux,
                    Language =  Language.Java,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new()
                {
                    CloudProvider = CloudProvider.Azure, // different than the main
                    HostingEnvironment =  HostEnvironment.Windows,
                    Language =  Language.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new()
                {
                    CloudProvider = CloudProvider.Azure, // different than the main
                    HostingEnvironment =  HostEnvironment.Windows,
                    Language =  Language.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
                new()
                {
                    CloudProvider = CloudProvider.Azure, // different than the main
                    HostingEnvironment =  HostEnvironment.Windows,
                    Language =  Language.Csharp,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);
            
            //  Assert
            Assert.NotNull(benchMarkData);
            Assert.Equal(CloudProvider.Firebase.ToString(), benchMarkData.CloudProvider);
            Assert.Equal(HostEnvironment.Linux.ToString(), benchMarkData.HostingEnvironment);
            Assert.Equal(Language.Java.GetName(), benchMarkData.Language);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Set_Enum_Number_Into_BenchMarkData_When_RequestData_Is_Outside_Of_EnumRange()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "2");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CloudProvider =  (CloudProvider)(-1) , // does not exist on enum
                    HostingEnvironment =(HostEnvironment)100, // does not exist on enum
                    Language = (Language)789, // does not exist on enum
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                },
            };

            var responseConverter = new ResponseConverterService();

            //  Act
            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            //  Assert
            Assert.NotNull(benchMarkData);
            Assert.Null(benchMarkData.CloudProvider);
            Assert.Null(benchMarkData.HostingEnvironment);
            Assert.Null(benchMarkData.Language);


        }
        [Fact]
        public void ConvertBenchMarkResult_Throws_InvalidOperationException_when_No_Results_Given()
        {
            //  Arrange

            var benchMarkResults = new List<BenchMarkResult>() { };

            var responseConverter = new ResponseConverterService();



            //  Act & Assert

            Assert.Throws<InvalidOperationException>(() => responseConverter.ConvertToBenchMarkData(benchMarkResults));


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Convert_BenchMarkResult_Date_With_The_Enviroment_DataRange_Into_DataPoints_Regardless_Of_Cold_Or_Warm_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "20");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)), // we need a date and as such a singular result
                    IsColdRequest = true// even though this is a cold request warm data points are still added
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            // 20 of the date range
            Assert.Equal(20, benchMarkData.ColdDataPoints.Count);
            Assert.Equal(20, benchMarkData.WarmDataPoints.Count);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Cold_Or_Warm_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(4, benchMarkData.ColdDataPoints[0].ExecutionTimes.Count);
            Assert.Single(benchMarkData.WarmDataPoints[0].ExecutionTimes);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Cold_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = true
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(4, benchMarkData.ColdDataPoints[0].ExecutionTimes.Count);
            Assert.Empty(benchMarkData.WarmDataPoints[0].ExecutionTimes);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Add_BenchMarkResult_DataRows_Into_DataPoints_By_Warm_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");



            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Empty(benchMarkData.ColdDataPoints[0].ExecutionTimes);
            Assert.Equal(4, benchMarkData.WarmDataPoints[0].ExecutionTimes.Count);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Calculate_Correct_Medians_By_Warm_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");



            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 120,
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 160,
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)), //today
                    RequestDuration = 84,
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 42,
                    IsColdRequest = false
                },
                // cold request that can be ignored and should not be taken into the medians
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 1220,
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 8160,
                    IsColdRequest = true
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(63, benchMarkData.WarmMedianExecutionTime); // 42-84 = 63 median
            Assert.Equal(-122.22, benchMarkData.WarmPreviousDayDifference);// (42-84)(63) vs (120-160)(140) = -77 diff = -122.22%
            Assert.True(benchMarkData.WarmPreviousDayPositive);


        }
        [Fact]
        public void ConvertBenchMarkResult_Should_Calculate_Correct_Medians_By_Cold_Status()
        {
            //  Arrange

            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 120,
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)),//yesterday
                    RequestDuration = 160,
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)), //today
                    RequestDuration = 84,
                    IsColdRequest = true
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 42,
                    IsColdRequest = true
                },

                // warm request that can be ignored and should not be taken into the medians
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,1)), //yesterday
                    RequestDuration = 7684,
                    IsColdRequest = false
                },
                new()
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020,1,2)),//today
                    RequestDuration = 9942,
                    IsColdRequest = false
                },
            };

            var responseConverter = new ResponseConverterService();



            //  Act

            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(63, benchMarkData.ColdMedianExecutionTime); // 42-84 = 63 median
            Assert.Equal(-122.22, benchMarkData.ColdPreviousDayDifference);// (42-84)(63) vs (120-160)(140) = -77 diff = -122.22%
            Assert.True(benchMarkData.ColdPreviousDayPositive);


        }

        [Fact]
        public void ConvertToBenchMarkData_Should_Exclude_Outliers_From_BenchmarkResult()
        {
            //  Arrange
            Environment.SetEnvironmentVariable("dayRange", "1");

            var benchMarkResults = new List<BenchMarkResult>();

            for (int i = 0; i < 10; i++)
            {
                benchMarkResults.Add(new BenchMarkResult
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020, 1, 1)), //yesterday
                    RequestDuration = 120,
                    IsColdRequest = false
                });

                benchMarkResults.Add(new BenchMarkResult
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020, 1, 2)), //today
                    RequestDuration = 120,
                    IsColdRequest = false
                });

                benchMarkResults.Add(new BenchMarkResult
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020, 1, 1)), //yesterday
                    RequestDuration = 120,
                    IsColdRequest = true
                });

                benchMarkResults.Add(new BenchMarkResult
                {
                    CreatedAt = new DateTimeOffset(new DateTime(2020, 1, 2)), //today
                    RequestDuration = 120,
                    IsColdRequest = true
                });
            }

            benchMarkResults.Add(new BenchMarkResult
            {
                CreatedAt = new DateTimeOffset(new DateTime(2020, 1, 2)), //today
                RequestDuration = 1,
                IsColdRequest = false
            });

            var responseConverter = new ResponseConverterService();


            //  Act
            var benchMarkData = responseConverter.ConvertToBenchMarkData(benchMarkResults);

            //  Assert
            Assert.NotNull(benchMarkData);

            // The benchMarkData object should not have a ColdDataPoint with an execution time of 1
            foreach (var execTime in benchMarkData.ColdDataPoints.Select(dataPoint => dataPoint.ExecutionTimes.FirstOrDefault(et => et == 1)))
            {
                Assert.Equal(0, execTime);
            }
        }
    }
}