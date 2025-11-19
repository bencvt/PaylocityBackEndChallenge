using Api.Dtos.Deduction;

namespace Api.Dtos.Paycheck;

public class GetPaycheckDto
{
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public DateTime CheckDate { get; set; }
    public int PayPeriod { get; set; }
    public ICollection<GetDeductionDto> Deductions { get; set; } = new List<GetDeductionDto>();
}
