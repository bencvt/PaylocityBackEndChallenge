using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Dtos.Deduction;

public class GetDeductionDto
{
    public decimal Amount { get; set; }
    public DeductionReason DeductionReason { get; set; }
    public GetDependentDto? Dependent { get; set; }
}
