using Api.Models;

namespace Api.Calculators;

public class PaycheckCalculator
{
    private Employee _employee;
    private DateTime _date;

    public PaycheckCalculator(Employee employee, DateTime date)
    {
        _employee = employee;
        _date = date;
    }

    public Paycheck Calculate()
    {
        decimal grossPay = Spread(_employee.Salary);
        var deductions = CreateDeductions();
        decimal netPay = grossPay - deductions.Sum(x => x.Amount);
        int payCycle = 0; // TODO

        return new()
        {
            GrossPay = grossPay,
            NetPay = netPay,
            Date = _date,
            PayCycle = payCycle,
            Deductions = deductions,
        };
    }

    private List<Deduction> CreateDeductions()
    {
        var result = new List<Deduction>();
        // TODO
        return result;
    }

    private static decimal Spread(decimal amount)
    {
        return amount / 26;
    }
}
