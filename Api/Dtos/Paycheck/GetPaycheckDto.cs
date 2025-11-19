using Api.Dtos.Deduction;

namespace Api.Dtos.Paycheck;

public class GetPaycheckDto
{
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public DateTime Date { get; set; }
    public int PayCycle { get; set; }
    public ICollection<GetDeductionDto> Deductions { get; set; } = new List<GetDeductionDto>();
}
