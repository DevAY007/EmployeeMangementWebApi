using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto.MedicalStatus;
using EmployeeManagementLibrary.Services.MedicalStatusService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalStatusController : ControllerBase
    {
        private readonly IMedicalStatusService _medicalStatus;

        public MedicalStatusController(IMedicalStatusService medicalStatus)
        {
            _medicalStatus = medicalStatus;
        }

        [HttpPost ("add-medicalRecord")]
        public async Task<IActionResult> AddRecord([FromBody] AddMedicalStatusDto request)
        {
            var result = await _medicalStatus.AddMedicalStatusAsync(request);
            if (result.IsSuccessful)
            {
                return StatusCode(200,request);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpPut("update-medicalRecord/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalStatusDto request)
        {
            var result = await _medicalStatus.UpdateMedicalStatusAsync(id, request);
            if (result.IsSuccessful)
            {
                return StatusCode(200, request);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpGet("medicalRecord/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _medicalStatus.GetMedicalStatusAsync(id);
            if(result.IsSuccessful)
            {
                return StatusCode(200);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _medicalStatus.DeleteMedicalStatusAsync(id);
           if(result.IsSuccessful)
           {
            return StatusCode(200);
           }
           return BadRequest(new {message = result.Message});
        }
    }
}