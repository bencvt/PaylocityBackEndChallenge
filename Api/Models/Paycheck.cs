namespace Api.Models;

public class Paycheck
{
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public DateTime Date { get; set; }
    public int PayCycle { get; set; }
    public ICollection<Deduction> Deductions { get; set; } = new List<Deduction>();
}
