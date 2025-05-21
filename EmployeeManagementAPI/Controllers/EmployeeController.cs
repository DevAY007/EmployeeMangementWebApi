using Microsoft.AspNetCore.Mvc;
using EmployeeManagementLibrary.Services.EmployeeServices;
using EmployeeManagementLibrary.Dto.Employees;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllByCompany([FromQuery] Guid companyId)
        {
            var result = await _employeeService.GetAllEmployeesByCompanyIdAsync(companyId);
            return result.IsSuccessful 
                ? Ok(result.Data) 
                : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _employeeService.GetEmployeeAsync(id);
            return result.IsSuccessful 
                ? Ok(result.Data) 
                : NotFound(result.Message);
        }

        [HttpPost("add-employee")]
        public async Task<IActionResult> Create([FromBody] AddEmployeeDto request)
        {
            var result = await _employeeService.AddEmployeeAsync(request);
            if (result.IsSuccessful)
            {
                return StatusCode(200,result);
            }
            return BadRequest(new { message = result.Message });
        }

        [HttpPut("update-employee/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto request, [FromHeader] Guid companyId)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, request, companyId);
            return result.IsSuccessful 
                ? Ok(result.Data) 
                : BadRequest(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid companyId)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id, companyId);
            return result.IsSuccessful 
                ? NoContent() 
                : NotFound(result.Message);
        }
    }
}
