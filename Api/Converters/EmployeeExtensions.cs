using Api.Dtos.Employee;
using Api.Models;

namespace Api.Converters;

public static class EmployeeExtensions
{
    public static GetEmployeeDto ConvertToGetEmployeeDto(this Employee source) => new()
    {
        Id = source.Id,
        FirstName = source.FirstName,
        LastName = source.LastName,
        Salary = source.Salary,
        DateOfBirth = source.DateOfBirth,
        Dependents = source.Dependents.ConvertToGetDependentDtoList(),
    };

    public static List<GetEmployeeDto> ConvertToGetEmployeeDtoList(this IEnumerable<Employee> source) => source
        .Select(x => x.ConvertToGetEmployeeDto())
        .ToList();
}
