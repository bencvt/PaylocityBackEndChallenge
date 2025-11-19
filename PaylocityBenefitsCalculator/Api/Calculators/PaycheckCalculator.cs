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
    private readonly CalculatorConfiguration _settings;
    private readonly Employee _employee;
    private readonly DateTime _checkDate;
    private readonly int _payPeriod;

    public PaycheckCalculator(CalculatorConfiguration settings, Employee employee, DateTime date)
    {
        _settings = settings;
        _employee = employee;

        var payPeriodCalculator = new PayPeriodCalculator(settings);
        _checkDate = payPeriodCalculator.GetLastDayInPayPeriod(date);
        _payPeriod = payPeriodCalculator.GetPayPeriod(_checkDate);
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

        DependentValidator.ValidateDependents(_settings, _employee);

        AddDependentBenefitsDeductions(deductions);

        AddHighIncomeDeductionIfNeeded(deductions);

        AddDependentAgeDeductionsIfNeeded(deductions);

        return deductions;
    }

    private void AddBaseBenefitsDeduction(List<Deduction> deductions)
    {
        deductions.Add(new()
        {
            Amount = SpreadAndRound(_settings.BaseBenefitsDeductionAmount),
            DeductionReason = DeductionReason.BaseBenefits,
        });
    }

    private void AddDependentBenefitsDeductions(List<Deduction> deductions)
    {
        foreach (var dependent in _employee.Dependents)
        {
            deductions.Add(new()
            {
                Amount = SpreadAndRound(_settings.DependentDeductionAmount),
                DeductionReason = DeductionReason.DependentBenefits,
                Dependent = dependent,
            });
        }
    }

    private void AddHighIncomeDeductionIfNeeded(List<Deduction> deductions)
    {
        if (_employee.Salary > _settings.HighIncomeDeductionSalaryThreshold)
        {
            deductions.Add(new()
            {
                Amount = SpreadAndRound(_employee.Salary * _settings.HighIncomeDeductionPercentage),
                DeductionReason = DeductionReason.HighIncome,
            });
        }
    }

    private void AddDependentAgeDeductionsIfNeeded(List<Deduction> deductions)
    {
        foreach (var dependent in _employee.Dependents)
        {
            if (AgeCalculator.GetAgeInYears(_checkDate, dependent.DateOfBirth) > _settings.DependentAgeDeductionYearsOldThreshold)
            {
                deductions.Add(new()
                {
                    Amount = SpreadAndRound(_settings.DependentAgeDeductionAmount),
                    DeductionReason = DeductionReason.DependentAge,
                    Dependent = dependent,
                });
            }
        }
    }

    private decimal SpreadAndRound(decimal amount)
    {
        return RoundingCalculator.SpreadAndRound(_settings, amount, _payPeriod);
    }
}
