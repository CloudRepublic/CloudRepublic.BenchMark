using CloudRepublic.BenchMark.API.V2.Statics;
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
            //  Arrange

            var currentDate = new DateTime(2020, 2, 2);

            var benchMarkResults = new List<BenchMarkResult>()
            {
            };



            //  Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(0, benchMarkData.CurrentDay);
            Assert.Equal(0, benchMarkData.PreviousDay);
            Assert.Equal(0, benchMarkData.DifferencePercentage); // for the difference an divide by currentday exists so make so it doesnt crash ;)


        }
        [Fact]
        public void Calculate_Should_Return_Zero_Medians_On_No_results_of_GivenDate_Or_DateBefore_GivenDate()
        {
            //  Arrange

            var currentDate = new DateTime(2020, 2, 12);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,13)), // tomorrow
                },
                new()
                {
                    RequestDuration = 200,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // not yesterday but before
                },
            };



            //  Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(0, benchMarkData.CurrentDay);
            Assert.Equal(0, benchMarkData.PreviousDay);
            Assert.Equal(0, benchMarkData.DifferencePercentage); // for the difference an divide by currentday exists so make so it doesnt crash ;)


        }

        [Fact]
        public void Calculate_Should_Return_Calculated_CurrentMedian_From_Results_By_Dates_Matching_Given()
        {
            //  Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new()
                {
                    RequestDuration = 33,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new()
                {
                    RequestDuration = 200,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };



            //  Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(22, benchMarkData.CurrentDay);


        }
        [Fact]
        public void Calculate_Should_Return_Calculated_PreviousMedian_From_Results_By_Dates_Matching_Day_Before_Given()
        {
            //  Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    RequestDuration = 234,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
                new()
                {
                    RequestDuration = 33,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };



            //  Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(22, benchMarkData.PreviousDay);


        }
        [Fact]
        public void Calculate_Should_Return_Calculated_Medians_From_Results_By_Dates_()
        {
            //  Arrange

            var currentDate = new DateTime(2020, 2, 11);

            var benchMarkResults = new List<BenchMarkResult>()
            {
                new()
                {
                    RequestDuration = 80,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new()
                {
                    RequestDuration = 20,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,11)), // today
                },
                new()
                {
                    RequestDuration = 11,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
                new()
                {
                    RequestDuration = 33,
                    CreatedAt = new DateTimeOffset(new DateTime(2020,2,10)), // yesterday
                },
            };



            //  Act

            var benchMarkData = MedianCalculator.Calculate(currentDate, benchMarkResults);



            //  Assert

            Assert.NotNull(benchMarkData);
            Assert.Equal(50, benchMarkData.CurrentDay);
            Assert.Equal(22, benchMarkData.PreviousDay);
            Assert.Equal(56, benchMarkData.DifferencePercentage);


        }
    }
}