using Api.Calculators;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests.Calculators;

public class PaycheckCalculatorTests
{
    [Fact]
    public void WhenCalculatingPaycheckWithInvalidDependentSet_ShouldThrowArgumentException()
    {
        var employee = new Employee()
        {
            Salary = 77_125m,
        };
        employee.Dependents.Add(new() { Relationship = Relationship.Spouse });
        employee.Dependents.Add(new() { Relationship = Relationship.Spouse });
        var calculator = new PaycheckCalculator(employee, DateTime.Parse("2025-12-25"));

        Assert.Throws<ArgumentException>(() => calculator.Calculate());
    }

    [Fact]
    public void WhenCalculatingPaycheckWithNoDependents_ShouldGenerateCorrectPaycheck()
    {
        var employee = new Employee()
        {
            Salary = 77_125m,
        };
        var calculator = new PaycheckCalculator(employee, DateTime.Parse("2025-12-25"));

        var actual = calculator.Calculate();

        var expected = new Paycheck()
        {
            GrossPay = 2966.3461538461538461538461538m,
            NetPay = 2927.8846153846153846153846153m,
            CheckDate = DateTime.Parse("2025-12-31"),
            PayPeriod = 26,
            Deductions = new List<Deduction>()
            {
                new()
                {
                    Amount = 38.461538461538461538461538462m,
                    DeductionReason = DeductionReason.BaseBenefits,
                },
            },
        };
        AssertPaycheckIs(expected, actual);
    }


    [Fact]
    public void WhenCalculatingPaycheckWithDependentsAndHighSalary_ShouldGenerateCorrectPaycheck()
    {
        var employee = new Employee()
        {
            Salary = 177_125m,
        };
        var childA = new Dependent()
        {
            FirstName = "Alice",
            Relationship = Relationship.Child,
            DateOfBirth = DateTime.Parse("2011-08-19"),
        };
        var childB = new Dependent()
        {
            FirstName = "Bob",
            Relationship = Relationship.Child,
            DateOfBirth = DateTime.Parse("2012-09-02"),
        };
        var spouse = new Dependent()
        {
            FirstName = "Dave",
            Relationship = Relationship.Child,
            DateOfBirth = DateTime.Parse("1974-01-02"),
        };
        employee.Dependents = new List<Dependent> { childA, spouse, childB };
        var calculator = new PaycheckCalculator(employee, DateTime.Parse("2025-05-05"));

        var actual = calculator.Calculate();

        var expected = new Paycheck()
        {
            GrossPay = 6812.5m,
            NetPay = 6560.8653846153846153846153846m,
            CheckDate = DateTime.Parse("2025-05-06"),
            PayPeriod = 9,
            Deductions = new List<Deduction>()
            {
                new()
                {
                    Amount = 38.461538461538461538461538462m,
                    DeductionReason = DeductionReason.BaseBenefits,
                },
                new()
                {
                    Amount = 23.076923076923076923076923077m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = childA,
                },
                new()
                {
                    Amount = 23.076923076923076923076923077m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = spouse,
                },
                new()
                {
                    Amount = 23.076923076923076923076923077m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = childB,
                },
                new()
                {
                    Amount = 136.25m,
                    DeductionReason = DeductionReason.HighIncome,
                },
                new()
                {
                    Amount = 7.6923076923076923076923076923m,
                    DeductionReason = DeductionReason.DependentAge,
                    Dependent = spouse,
                },
            },
        };
        AssertPaycheckIs(expected, actual);
    }

    private static void AssertPaycheckIs(Paycheck expected, Paycheck actual)
    {
        Assert.Equal(expected.ToString(), actual.ToString());
        Assert.Equal(expected.GrossPay, actual.GrossPay);
        Assert.Equal(expected.NetPay, actual.NetPay);
        Assert.Equal(expected.CheckDate, actual.CheckDate);
        Assert.Equal(expected.PayPeriod, actual.PayPeriod);
        Assert.Equal(expected.Deductions.Count, actual.Deductions.Count);
        var expectedDeductions = expected.Deductions.ToList();
        var actualDeductions = actual.Deductions.ToList();
        for (var i = 0; i < expectedDeductions.Count; i++)
        {
            AssertDeductionIs(expectedDeductions[i], actualDeductions[i]);
        }
    }

    private static bool AssertDeductionIs(Deduction expected, Deduction actual)
    {
        Assert.Equal(expected.ToString(), actual.ToString());
        Assert.Equal(expected.Amount, actual.Amount);
        Assert.Equal(expected.DeductionReason, actual.DeductionReason);
        Assert.Equal(expected.Dependent, actual.Dependent);
        return true;
    }
}
