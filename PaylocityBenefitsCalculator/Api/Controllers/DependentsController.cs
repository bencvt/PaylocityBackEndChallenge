using Api.Converters;
using Api.Dtos.Dependent;
using Api.Models;
using Api.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            var retriever = new DependentRetriever();
            var dependent = retriever.RetrieveById(id);
            var data = dependent.ConvertToGetDependentDto();
            return Ok(new ApiResponse<GetDependentDto>
            {
                Data = data,
                Success = true,
            });
        }
        catch (Exception ex)
        {
            return NotFound(new ApiResponse<GetDependentDto>()
            {
                Error = ex.Message,
                Success = false,
            });
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        try
        {
            var retriever = new DependentRetriever();
            var dependents = retriever.RetrieveAll();
            var data = dependents.ConvertToGetDependentDtoList();
            return Ok(new ApiResponse<List<GetDependentDto>>
            {
                Data = data,
                Success = true,
                Message = $"{data.Count} dependents found",
            });
        }
        catch (Exception ex)
        {
            return NotFound(new ApiResponse<List<GetDependentDto>>
            {
                Error = ex.Message,
                Success = false,
            });
        }
    }
}