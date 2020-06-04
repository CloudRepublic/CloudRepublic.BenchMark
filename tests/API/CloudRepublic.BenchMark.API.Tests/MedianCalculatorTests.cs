﻿using CloudRepublic.BenchMark.API.Helpers;
using CloudRepublic.BenchMark.Domain.Entities;
using System;
using System.Collections.Generic;
using Xunit;


namespace CloudRepublic.BenchMark.API.Tests
{
    public class MedianCalculatorTests
    {
        [Fact]
        public void ConvertBenchMarkResult_Should_Return_Empty_Medians_On_No_results()
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
            Assert.Equal(0, benchMarkData.Difference); // for the difference an divide by currentday exists so make so it doesnt crash ;)

            #endregion
        }

    }
}
