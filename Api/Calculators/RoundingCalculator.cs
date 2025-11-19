namespace Api.Calculators;

public static class RoundingCalculator
{
    /// <summary>
    /// Given an annual amount, calculate the corresponding amount per pay period.
    /// <br/>
    /// Rounds the amount to the nearest cent.
    /// <br/>
    /// The calculation for the final pay period of the year corrects any
    /// accumulated rounding errors.
    /// </summary>
    public static decimal SpreadAndRound(decimal amount, int payPeriod)
    {
        decimal numPeriods = PayPeriodCalculator.NUM_PAY_PERIODS_PER_YEAR;

        // Math.Floor here is an arbitrary choice.
        // It might make more sense to use Math.Round, or Math.Ceiling,
        // or the rounding logic could depend on the type of amount (gross vs deduction).
        decimal actualPortion = Math.Floor(amount * 100m / numPeriods) / 100m;

        if (payPeriod == numPeriods)
        {
            // Any rounding errors are adjusted in the final pay period of the year.
            decimal precisePortion = amount / numPeriods;
            decimal remainder = (precisePortion - actualPortion) * numPeriods;
            actualPortion += Math.Round(remainder * 100m) / 100m;
        }
        return actualPortion;
    }
}
