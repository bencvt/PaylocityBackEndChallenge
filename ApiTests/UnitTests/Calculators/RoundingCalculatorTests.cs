using Api.Calculators;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.UnitTests.Calculators;

public class RoundingCalculatorTests
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void WhenSpreadingAndRounding_ShouldReturnCorrectValue(decimal amount, int payPeriod, decimal expected)
    {
        decimal actual = RoundingCalculator.SpreadAndRound(amount, payPeriod);

        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TestCases => new List<object[]>
    {
        new object[] { 321.77m, 1, 12.37m },
        new object[] { 321.77m, 2, 12.37m },
        new object[] { 321.77m, 3, 12.37m },
        new object[] { 321.77m, 24, 12.37m },
        new object[] { 321.77m, 25, 12.37m },
        new object[] { 321.77m, 26, 12.52m },
    };
}
