namespace Api.Calculators;

public class PayPeriodCalculator
{
    private readonly CalculatorConfiguration _settings;

    public PayPeriodCalculator(CalculatorConfiguration settings)
    {
        _settings = settings;
    }

    public int GetPayPeriod(DateTime date)
    {
        return (int)Math.Ceiling(date.DayOfYear * (double)_settings.NumPayPeriodsPerYear / 365);
    }

    public DateTime GetLastDayInPayPeriod(DateTime date)
    {
        int payPeriod = GetPayPeriod(date);
        var last = date;
        while (true)
        {
            // AddDays assumes 24 hour clock (no DST), so this is safe
            // even if date has hh:mm:ss components.
            var next = last.AddDays(1);
            if (GetPayPeriod(next) != payPeriod)
            {
                return last;
            }
            last = next;
        }
    }
}
