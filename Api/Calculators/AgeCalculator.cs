namespace Api.Calculators;

public static class AgeCalculator
{
    public static int GetAgeInYears(DateTime effectiveDate, DateTime dateOfBirth)
    {
        int years = effectiveDate.Year - dateOfBirth.Year;
        if (effectiveDate.Month < dateOfBirth.Month
            || (effectiveDate.Month == dateOfBirth.Month && effectiveDate.Day < dateOfBirth.Day))
        {
            years--;
        }
        return Math.Max(0, years);
    }
}
