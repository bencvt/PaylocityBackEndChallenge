using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Converters;

public static class PaycheckExtensions
{
    public static GetPaycheckDto ConvertToGetPaycheckDto(this Paycheck source) => new()
    {
        GrossPay = source.GrossPay,
        NetPay = source.NetPay,
        CheckDate = source.CheckDate,
        PayPeriod = source.PayPeriod,
        Deductions = source.Deductions.ConvertToGetDeductionDtoList(),
    };

    public static List<GetPaycheckDto> ConvertToGetPaycheckDtoList(this IEnumerable<Paycheck> source) => source
        .Select(x => x.ConvertToGetPaycheckDto())
        .ToList();
}
