namespace Api;

public class CalculatorConfiguration
{
    public bool EnforceMaxOneSpouseOrDomesticPartner { get; set; } = true;

    public int NumPayPeriodsPerYear { get; set; } = 26;

    public decimal BaseBenefitsDeductionAmount { get; set; } = 1_000.00m;

    public decimal DependentDeductionAmount { get; set; } = 600.00m;

    public decimal HighIncomeDeductionSalaryThreshold { get; set; } = 80_000.00m;

    public decimal HighIncomeDeductionPercentage { get; set; } = 0.02m;

    public int DependentAgeDeductionYearsOldThreshold { get; set; } = 50;

    public decimal DependentAgeDeductionAmount { get; set; } = 200.00m;
}
