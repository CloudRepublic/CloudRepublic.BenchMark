using System;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.API.V2.Statics;
using CloudRepublic.BenchMark.Domain.Entities;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests;

public class OutlierCalculatorTests
{
    [Fact]
    public void Calculate_Should_Return_No_Outliers()
    {
        Environment.SetEnvironmentVariable("dayRange", "2");

        // Arrange
        var currentDate = new DateTime(2025, 4, 9);

        var results = new List<BenchMarkResult>
        {
            new()
            {
                RequestDuration = 70,
                CreatedAt = currentDate
            },
            new()
            {
                RequestDuration = 71,
                CreatedAt = currentDate
            },
            new()
            {
                RequestDuration = 72,
                CreatedAt = currentDate
            }
        };

        // Act
        var outliers = OutliersCalculator.Calculate(currentDate, results);

        // Arrange
        Assert.NotNull(outliers);
        Assert.Empty(outliers.OutliersPerDay[currentDate]);
    }

    [Fact]
    public void Calculate_Should_Return_Outliers()
    {
        Environment.SetEnvironmentVariable("dayRange", "2");

        // Arrange
        var currentDate = new DateTime(2025, 4, 9);
        var yesterday = currentDate.AddDays(-1);

        var results = new List<BenchMarkResult>();

        for (int i = 0; i < 10; i++)
        {
            results.Add( new()
            {
                RequestDuration = 2,
                CreatedAt = currentDate
            });
        }
        for (int i = 0; i < 10; i++)
        {
            results.Add(new()
            {
                RequestDuration = 2,
                CreatedAt = yesterday
            });
        }

        results.Add(new()
        {
            RequestDuration = 1000,
            CreatedAt = currentDate
        });
        results.Add(new()
        {
            RequestDuration = 1000,
            CreatedAt = yesterday
        });

        // Act
        var outliers = OutliersCalculator.Calculate(currentDate, results);

        // Arrange
        Assert.NotNull(outliers);
        Assert.Equal(1000, outliers.OutliersPerDay[yesterday].First());
        Assert.Equal(1000, outliers.OutliersPerDay[currentDate].First());
    }

}