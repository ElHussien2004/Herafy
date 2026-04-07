using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOs.UserDTOS;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthService(IConnectionMultiplexer connection,ISMSService _SMS,UserManager<ApplicationUser> userManager,IConfiguration _configuration) : IAuthService
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<Result> SendOtpAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Error.Validation("Phone.Empty", "رقم الهاتف مطلوب");

            var key = $"otp:{phoneNumber}";

            var existingOtp = await _database.StringGetAsync(key);
            if (!existingOtp.IsNullOrEmpty)
                return Error.Failure("OTP.Exists", "تم إرسال كود بالفعل، حاول لاحقًا");

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            await _database.StringSetAsync(key, otp, TimeSpan.FromMinutes(10));

            var message = $"[Herafy] Your OTP code is {otp}. Valid for 10 minutes.";

            var sent = await _SMS.SendAsync(phoneNumber, message);

            if (!sent.IsSuccess)
            {
                await _database.KeyDeleteAsync(key);
                return Error.Failure("SMS.Failed", "فشل في إرسال الرسالة");

            }
            return Result.Ok();
        }

        public async Task<Result<AuthResultDto>> VerifyOtpAsync(string phoneNumber, string otpCode)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(otpCode))
                return Error.Validation("OTP.InvalidInput", "بيانات غير صحيحة");

            var key = $"otp:{phoneNumber}";

            var storedOtp = await _database.StringGetAsync(key);

            if (storedOtp.IsNullOrEmpty)
                return Error.NotFound("OTP.Expired", "انتهت صلاحية الكود");

            if (storedOtp != otpCode)
                return Error.InvalidCrendentials("OTP.Wrong", "الكود غير صحيح");

            await _database.KeyDeleteAsync(key);

            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = phoneNumber,
                    PhoneNumber = phoneNumber
                };

                var createResult = await userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                    return Error.Failure("User.CreateFailed", "فشل إنشاء المستخدم");
            }

           var token = await CreateTokenAsync(user);

            return Result<AuthResultDto>.Ok( new AuthResultDto
            {
                UserId= user.Id,
                IsAuthenticated = true,
                Token = token,
                PhoneNumber = phoneNumber
            });
        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>(){
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new ("TokenId",Guid.NewGuid().ToString()),
                new (ClaimTypes.MobilePhone ,user.PhoneNumber)
                };

            var Roles = await userManager.GetRolesAsync(user);

            foreach (var role in Roles)
                Claims.Add(new Claim(ClaimTypes.Role, role));

            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Cards = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTOptions")["issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: Claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: Cards
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
