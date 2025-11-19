using Api.Converters;
using Api.Dtos.Employee;
using Api.Models;
using Api.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            var retriever = new EmployeeRetriever();
            var employees = retriever.RetrieveAll();
            var data = employees.ConvertToGetEmployeeDtoList();
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = data,
                Success = true,
                Message = $"{data.Count} employees found",
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Error = ex.Message,
                Success = false,
            };
        }
    }
}
