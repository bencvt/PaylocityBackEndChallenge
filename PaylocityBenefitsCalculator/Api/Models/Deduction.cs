namespace Api.Models;

public class Deduction
{
    public decimal Amount { get; set; }
    public DeductionReason DeductionReason { get; set; }
    public Dependent? Dependent { get; set; }
}
