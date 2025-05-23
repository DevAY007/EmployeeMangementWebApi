using EmployeeManagementLibrary.Services.CompanyService;
using EmployeeManagementLibrary.Dto.Company;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyService.GetAllCompanyAsync();
            return result.IsSuccessful ? Ok(result.Data) : StatusCode(500, result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyDetails(Guid id)
        {
            var result = await _companyService.GetCompanyAsync(id);
            if (!result.IsSuccessful) return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto request)
        {
            var result = await _companyService.CreateCompanyAsync(request);

            if (result.IsSuccessful)
            {
                return StatusCode(200,result);
            }
            return BadRequest(result.Message);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto request)
        {
            var result = await _companyService.UpdateCompanyAsync(id, request);
            return result.IsSuccessful ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            return result.IsSuccessful ? NoContent() : NotFound(result.Message);
        }
    }
}
