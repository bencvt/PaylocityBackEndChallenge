using Api.Models;

namespace Api.Calculators;

/// <summary>
/// Generate a paycheck for the specified employee id, effective for the
/// last day in the pay period containing the specified date.
/// <para/>
/// Throw an exception if the employee record has an invalid set of dependents.
/// </summary>
public class PaycheckCalculator
{
    private readonly Employee _employee;
    private readonly DateTime _checkDate;
    private readonly int _payPeriod;

    public PaycheckCalculator(Employee employee, DateTime date)
    {
        _employee = employee;
        _checkDate = PayPeriodCalculator.GetLastDayInPayPeriod(date);
        _payPeriod = PayPeriodCalculator.GetPayPeriod(_checkDate);
    }

    public Paycheck Calculate()
    {
        decimal grossPay = SpreadAndRound(_employee.Salary);
        var deductions = CreateDeductions();
        decimal netPay = grossPay - deductions.Sum(x => x.Amount);

        return new()
        {
            GrossPay = grossPay,
            NetPay = netPay,
            CheckDate = _checkDate,
            PayPeriod = _payPeriod,
            Deductions = deductions,
        };
    }

    private List<Deduction> CreateDeductions()
    {
        var deductions = new List<Deduction>();

        AddBaseBenefitsDeduction(deductions);

        DependentValidator.ValidateDependents(_employee);

        AddDependentBenefitsDeductions(deductions);

        AddHighIncomeDeductionIfNeeded(deductions);

        AddDependentAgeDeductionsIfNeeded(deductions);

        return deductions;
    }

    private void AddBaseBenefitsDeduction(List<Deduction> deductions)
    {
        deductions.Add(new()
        {
            Amount = SpreadAndRound(1_000m),
            DeductionReason = DeductionReason.BaseBenefits,
        });
    }

    private void AddDependentBenefitsDeductions(List<Deduction> deductions)
    {
        foreach (var dependent in _employee.Dependents)
        {
            deductions.Add(new()
            {
                Amount = SpreadAndRound(600m),
                DeductionReason = DeductionReason.DependentBenefits,
                Dependent = dependent,
            });
        }
    }

    private void AddHighIncomeDeductionIfNeeded(List<Deduction> deductions)
    {
        if (_employee.Salary > 80_000m)
        {
            deductions.Add(new()
            {
                Amount = SpreadAndRound(_employee.Salary * 0.02m),
                DeductionReason = DeductionReason.HighIncome,
            });
        }
    }

    private void AddDependentAgeDeductionsIfNeeded(List<Deduction> deductions)
    {
        foreach (var dependent in _employee.Dependents)
        {
            if (AgeCalculator.GetAgeInYears(_checkDate, dependent.DateOfBirth) > 50)
            {
                deductions.Add(new()
                {
                    Amount = SpreadAndRound(200m),
                    DeductionReason = DeductionReason.DependentAge,
                    Dependent = dependent,
                });
            }
        }
    }

    private decimal SpreadAndRound(decimal amount)
    {
        return RoundingCalculator.SpreadAndRound(amount, _payPeriod);
    }
}
