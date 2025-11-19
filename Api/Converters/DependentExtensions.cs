using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Converters;

public static class DependentExtensions
{
    public static GetDependentDto ConvertToGetDependentDto(this Dependent source) => new()
    {
        Id = source.Id,
        FirstName = source.FirstName,
        LastName = source.LastName,
        DateOfBirth = source.DateOfBirth,
        Relationship = source.Relationship,
    };

    public static List<GetDependentDto> ConvertToGetDependentDtoList(this IEnumerable<Dependent> source) => source
        .Select(x => x.ConvertToGetDependentDto())
        .ToList();
}
