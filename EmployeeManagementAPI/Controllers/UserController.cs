using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EmployeeManagementLibrary.Services;
using EmployeeManagementLibrary.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UserRegistration(request);

            if (result.IsSuccessful)
            {
                return StatusCode(200,result);
            }
            return BadRequest(result.Message);
        }

        // GET: api/user/detail/{id}
    [HttpGet("detail/{id}")]
    public async Task<IActionResult> UserDetail(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        if (!result.IsSuccessful)
        {
            return NotFound("User not found.");
        }

        return Ok(result.Data);
    }

    //// GET: api/user/edit-user/{id} (optional - might not be needed for an API)
    //[HttpGet("edit-user/{id}")]
    //public async Task<IActionResult> EditUser(Guid id)
    //{
    //    var result = await _userService.GetUserByIdAsync(id);

    //    if (!result.IsSuccessful)
    //    {
    //        return NotFound("User not found.");
    //    }

    //    return Ok(result.Data);
    //}

    // PUT: api/user/edit-user/{id}
    [HttpPut("edit-user/{id}")]
    public async Task<IActionResult> EditUser(Guid id, [FromBody] UpdateUserDto request)
    {
        var result = await _userService.UpdateUserAsync(id, request);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return NoContent(); // 204 No Content (update successful, no response body needed)
    }

    // DELETE: api/user/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return NoContent(); // 204 No Content (successful deletion)
    }
    }
}