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
        Task<bool> SendOtpAsync(string phoneNumber);
        Task<AuthResultDto> VerifyOtpAsync(string phoneNumber, string otpCode);
    }
}
