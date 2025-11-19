using Api.Calculators;
using Api.Converters;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaychecksController : ControllerBase
{
    [SwaggerOperation(Summary = "Get paycheck by employee id and date")]
    [HttpGet("{employeeId}/{date}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int employeeId, DateTime date)
    {
        try
        {
            var retriever = new EmployeeRetriever();
            var employee = retriever.RetrieveById(employeeId);

            var calculator = new PaycheckCalculator(employee, date);
            var paycheck = calculator.Calculate();

            var data = paycheck.ConvertToGetPaycheckDto();
            return Ok(new ApiResponse<GetPaycheckDto>
            {
                Data = data,
                Success = true,
            });
        }
        catch (Exception ex)
        {
            return NotFound(new ApiResponse<GetPaycheckDto>()
            {
                Error = ex.Message,
                Success = false,
            });
        }
    }
}
