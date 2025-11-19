namespace Api.Calculators;

public static class PayPeriodCalculator
{
    public static int GetPayPeriod(DateTime date)
    {
        return (int)Math.Ceiling(date.DayOfYear * 26.0 / 365);
    }

    public static DateTime GetLastDayInPayPeriod(DateTime date)
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
