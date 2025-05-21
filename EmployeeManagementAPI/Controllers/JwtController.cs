using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Services;
using EmployeeManagementMVC.Models.JwtSettings;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public JwtController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result is null)
                return Unauthorized();

            return result;
        }
    }
}