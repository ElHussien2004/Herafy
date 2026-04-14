using Shared.CommonResult;
using Shared.DTOs;
using Shared.DTOs.UserDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<Result> SendOtpAsync(SendOtpDto dto);
        Task<Result<AuthResultDto>> VerifyOtpAsync(VerifyOtpDto dto);
    }
}
