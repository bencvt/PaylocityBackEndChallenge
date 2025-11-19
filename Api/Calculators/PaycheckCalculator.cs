using Api.Models;

namespace Api.Calculators;

public class PaycheckCalculator
{
    private readonly Employee _employee;
    private readonly DateTime _checkDate;

    public PaycheckCalculator(Employee employee, DateTime date)
    {
        _employee = employee;
        _checkDate = PayPeriodCalculator.GetLastDayInPayPeriod(date);
    }

    public Paycheck Calculate()
    {
        decimal grossPay = Spread(_employee.Salary);
        var deductions = CreateDeductions();
        decimal netPay = grossPay - deductions.Sum(x => x.Amount);
        int payPeriod = PayPeriodCalculator.GetPayPeriod(_checkDate);

        return new()
        {
            GrossPay = grossPay,
            NetPay = netPay,
            CheckDate = _checkDate,
            PayPeriod = payPeriod,
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

    private static void AddBaseBenefitsDeduction(List<Deduction> deductions)
    {
        deductions.Add(new()
        {
            Amount = Spread(1_000m),
            DeductionReason = DeductionReason.BaseBenefits,
        });
    }

    private void AddDependentBenefitsDeductions(List<Deduction> deductions)
    {
        foreach (var dependent in _employee.Dependents)
        {
            deductions.Add(new()
            {
                Amount = Spread(600),
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
                Amount = Spread(_employee.Salary * 0.02m),
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
                    Amount = Spread(200),
                    DeductionReason = DeductionReason.DependentAge,
                    Dependent = dependent,
                });
            }
        }
    }

    private static decimal Spread(decimal amount)
    {
        return amount / 26;
    }
}
