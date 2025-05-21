using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto.EducationRecord;
using EmployeeManagementLibrary.Services.EducationHistoryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EducationHistoryController : ControllerBase
    {
        private readonly IEducationHistoryService _educationRecord;

        public EducationHistoryController(IEducationHistoryService educationRecord)
        {
            _educationRecord = educationRecord;
        }

        [HttpPost ("add-education-records")]
        public async Task<IActionResult> AddRecord([FromBody] AddEducationRecordDto request)
        {
            var result = await _educationRecord.AddEducationRecordAsync(request);
            if (result.IsSuccessful)
            {
                return StatusCode(200,request);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpPut("update-education-records/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEducationHistoryDto request)
        {
            var result = await _educationRecord.UpdateEducationRecordAsync(id, request);
            if (result.IsSuccessful)
            {
                return StatusCode(200, request);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpGet("education-records/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _educationRecord.GetEducationRecordAsync(id);
            if(result.IsSuccessful)
            {
                return StatusCode(200);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _educationRecord.DeleteEducationRecordsAsync(id);
           if(result.IsSuccessful)
           {
            return StatusCode(200);
           }
           return BadRequest(new {message = result.Message});
        }
    }
}