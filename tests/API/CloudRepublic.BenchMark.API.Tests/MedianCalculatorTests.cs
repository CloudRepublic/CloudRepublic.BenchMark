using CloudRepublic.BenchMark.API.Helpers;
using CloudRepublic.BenchMark.Domain.Entities;
using System;
using System.Collections.Generic;
using Xunit;


namespace CloudRepublic.BenchMark.API.Tests
{
    public class MedianCalculatorTests
    {
        [Fact]
        public void Calculate_Should_Return_Zero_Medians_On_No_results()
        {
            #region Arrange

            var currentDate = new DateTime(2020, 2, 2);

            var benchMarkResults = new List<BenchMarkResult>()
            {
            };

            #endregion

            #region Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(0, benchMarkData.CurrentDay);
            Assert.Equal(0, benchMarkData.PreviousDay);
            Assert.Equal(0, benchMarkData.DifferencePercentage); // for the difference an divide by currentday exists so make so it doesnt crash ;)

            #endregion
        }
        [Fact]
        public void Calculate_Should_Return_Zero_Medians_On_No_results_of_GivenDate_Or_DateBefore_GivenDate()
        {
            #region Arrange

            var currentDate = new DateTime(2020, 2, 12);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,13)), // tomorrow
                },
                new BenchMarkResult()
                {
                    RequestDuration = 200,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // not yesterday but before
                },
            };

            #endregion

            #region Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(0, benchMarkData.CurrentDay);
            Assert.Equal(0, benchMarkData.PreviousDay);
            Assert.Equal(0, benchMarkData.DifferencePercentage); // for the difference an divide by currentday exists so make so it doesnt crash ;)

            #endregion
        }

        [Fact]
        public void Calculate_Should_Return_Calculated_CurrentMedian_From_Results_By_Dates_Matching_Given()
        {
            #region Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new BenchMarkResult()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new BenchMarkResult()
                {
                    RequestDuration = 200,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };

            #endregion

            #region Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(11, benchMarkData.CurrentDay);

            #endregion
        }
        [Fact]
        public void Calculate_Should_Return_Calculated_PreviousMedian_From_Results_By_Dates_Matching_Day_Before_Given()
        {
            #region Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    RequestDuration = 234,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new BenchMarkResult()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
                new BenchMarkResult()
                {
                    RequestDuration = 33,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };

            #endregion

            #region Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(22, benchMarkData.PreviousDay);

            #endregion
        }
        [Fact]
        public void Calculate_Should_Return_Calculated_Medians_From_Results_By_Dates_()
        {
            #region Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new BenchMarkResult()
                {
                    RequestDuration = 80,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new BenchMarkResult()
                {
                    RequestDuration = 20,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new BenchMarkResult()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
                new BenchMarkResult()
                {
                    RequestDuration = 33,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };

            #endregion

            #region Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);

            #endregion

            #region Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(50, benchMarkData.CurrentDay);
            Assert.Equal(22, benchMarkData.PreviousDay);
            Assert.Equal(56, benchMarkData.DifferencePercentage);

            #endregion
        }
    }
}
