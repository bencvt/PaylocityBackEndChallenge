using Api.Calculators;
using System;
using Xunit;

namespace ApiTests.UnitTests.Calculators;

public class PayPeriodCalculatorTests : UnitTest
{
    [Theory]
    [InlineData("2025-01-01", 1)]
    [InlineData("2025-01-02", 1)]
    [InlineData("2025-01-03", 1)]
    [InlineData("2025-01-14", 1)]
    [InlineData("2025-01-15", 2)]
    [InlineData("2025-01-16", 2)]
    [InlineData("2025-01-28", 2)]
    [InlineData("2025-01-29", 3)]
    [InlineData("2025-01-30", 3)]
    [InlineData("2025-01-31", 3)]
    [InlineData("2025-02-01", 3)]
    [InlineData("2025-05-05", 9)]
    [InlineData("2025-12-29", 26)]
    [InlineData("2025-12-30", 26)]
    [InlineData("2025-12-31", 26)]
    public void WhenAskedForPayPeriod_ShouldReturnCorrectValue(DateTime date, int expected)
    {
        var calculator = new PayPeriodCalculator(Settings);

        int actual = calculator.GetPayPeriod(date);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-01-01", "2025-01-14")]
    [InlineData("2025-01-02", "2025-01-14")]
    [InlineData("2025-01-03", "2025-01-14")]
    [InlineData("2025-01-14", "2025-01-14")]
    [InlineData("2025-01-15", "2025-01-28")]
    [InlineData("2025-01-16", "2025-01-28")]
    [InlineData("2025-01-28", "2025-01-28")]
    [InlineData("2025-01-29", "2025-02-11")]
    [InlineData("2025-01-30", "2025-02-11")]
    [InlineData("2025-01-31", "2025-02-11")]
    [InlineData("2025-02-01", "2025-02-11")]
    [InlineData("2025-05-05", "2025-05-06")]
    [InlineData("2025-12-29", "2025-12-31")]
    [InlineData("2025-12-30", "2025-12-31")]
    [InlineData("2025-12-31", "2025-12-31")]
    public void WhenAskedForLastDayInPayPeriod_ShouldReturnCorrectValue(DateTime date, DateTime expected)
    {
        var calculator = new PayPeriodCalculator(Settings);

        DateTime actual = calculator.GetLastDayInPayPeriod(date);

        Assert.Equal(expected, actual);
    }
}
