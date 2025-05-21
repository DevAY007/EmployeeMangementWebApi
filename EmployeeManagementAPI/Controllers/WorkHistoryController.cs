using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto.WorkHistory;
using EmployeeManagementLibrary.Services.WorkHistoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkHistoryController : ControllerBase
    {
        private readonly IWorkHistoryService _workHistoryService;

        public WorkHistoryController(IWorkHistoryService workHistoryService)
        {
            _workHistoryService = workHistoryService;
        }

        [HttpPost ("add-workhistory")]
        public async Task<IActionResult> AddRecord([FromBody] AddWorkHistoryDto request)
        {
            var result = await _workHistoryService.AddWorkHistoryAsync(request);

            if(result.IsSuccessful)
            {
                return StatusCode(200,request);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpGet ("workHisrory/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _workHistoryService.GetWorkHistoryAsync(id);

            if(result.IsSuccessful)
            {
                return StatusCode(200);
            }
            return BadRequest(new {message = result.Message});
        }

        [HttpPut ("update-workHistory/{id}")]
        public async Task<IActionResult> UpdateRecord(Guid id,[FromBody] UpdateWorkHistoryDto request)
        {
            var result = await _workHistoryService.UpdateWorkHistoryAsync(id, request);
            if(result.IsSuccessful)
            {
                return StatusCode(200, request);
            }
            return BadRequest(new{message = result.Message});
        }

         [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _workHistoryService.DeleteWorkHistoryAsync(id);
           if(result.IsSuccessful)
           {
            return StatusCode(200);
           }
           return BadRequest(new {message = result.Message});
        }

    }
}