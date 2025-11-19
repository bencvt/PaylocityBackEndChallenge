using Api.Calculators;
using System;
using Xunit;

namespace ApiTests.UnitTests.Calculators;

public class AgeCalculatorTests
{
    [Theory]
    [InlineData("2025-10-25", "2025-10-24", 0)]
    [InlineData("2025-10-25", "2025-10-25", 0)]
    [InlineData("2025-10-25", "2025-10-26", 0)]

    [InlineData("2025-10-25", "2000-01-01", 25)]
    [InlineData("2025-10-25", "2000-10-01", 25)]
    [InlineData("2025-10-25", "2000-10-24", 25)]
    [InlineData("2025-10-25", "2000-10-25", 25)]
    [InlineData("2025-10-25", "2000-10-26", 24)]
    [InlineData("2025-10-25", "2000-12-31", 24)]

    [InlineData("2025-01-01", "2000-08-11", 24)]
    [InlineData("2025-08-01", "2000-08-11", 24)]
    [InlineData("2025-08-10", "2000-08-11", 24)]
    [InlineData("2025-08-11", "2000-08-11", 25)]
    [InlineData("2025-08-12", "2000-08-11", 25)]
    [InlineData("2025-12-31", "2000-08-11", 25)]
    public void WhenAskedForAge_ShouldReturnCorrectAge(DateTime effectiveDate, DateTime dateOfBirth, int expected)
    {
        int actual = AgeCalculator.GetAgeInYears(effectiveDate, dateOfBirth);

        Assert.Equal(expected, actual);
    }
}
