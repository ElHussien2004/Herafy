using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.UserDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthController(IServiceManager _serviceManager):ApiBaseController
    {
        [HttpPost("send-otp")]
        public async Task<ActionResult<string>> SendOtp([FromBody] SendOtpDto dto)
        {
            var result = await _serviceManager.AuthService.SendOtpAsync(dto);

            return HandleResult(result);
        }
        [HttpPost("verify-otp")]
        public async Task<ActionResult<AuthResultDto>> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var result = await _serviceManager.AuthService.VerifyOtpAsync(dto);

            return HandleResult(result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ReturnAdminDto>> Login([FromBody] LoginAdminDto dto)
        {
            var result =await _serviceManager.AuthService.Login(dto);
            return HandleResult(result);
        }
    }
}
