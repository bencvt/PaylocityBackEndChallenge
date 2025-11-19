namespace Api.Models;

public class Paycheck
{
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public DateTime CheckDate { get; set; }
    public int PayPeriod { get; set; }
    public ICollection<Deduction> Deductions { get; set; } = new List<Deduction>();
}
