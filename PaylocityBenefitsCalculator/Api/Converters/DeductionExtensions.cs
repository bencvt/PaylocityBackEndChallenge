using Api.Dtos.Deduction;
using Api.Models;

namespace Api.Converters;

public static class DeductionExtensions
{
    public static GetDeductionDto ConvertToGetDeductionDto(this Deduction source) => new()
    {
        Amount = source.Amount,
        DeductionReason = source.DeductionReason,
        Dependent = source.Dependent?.ConvertToGetDependentDto(),
    };

    public static List<GetDeductionDto> ConvertToGetDeductionDtoList(this IEnumerable<Deduction> source) => source
        .Select(x => x.ConvertToGetDeductionDto())
        .ToList();
}
