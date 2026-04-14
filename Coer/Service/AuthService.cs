using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.CommonResult;
using Shared.DTOs;
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
using Twilio.Types;

namespace Service
{
    public class AuthService(IConnectionMultiplexer connection, ISMSService _SMS, UserManager<ApplicationUser> userManager, IConfiguration _configuration) : IAuthService
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<Result> SendOtpAsync(SendOtpDto dto)
        {
            // 1. Fail-Fast Validations
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                return Error.Validation("Phone.Empty", "رقم الهاتف مطلوب");

            if (dto.UserType.ToString() != "Technician" && dto.UserType.ToString() != "Client")
                return Error.Validation("UserType.Invalid", "نوع المستخدم غير صحيح");

            var otpKey = $"otp:{dto.PhoneNumber}";
            var typeKey = $"type:{dto.PhoneNumber}";

            var existingOtp = await _database.StringGetAsync(otpKey);
            if (!existingOtp.IsNullOrEmpty)
                return Error.Failure("OTP.Exists", "تم إرسال كود بالفعل، حاول لاحقًا");

            async Task RevertRedisKeysAsync()
            {
                await _database.KeyDeleteAsync(otpKey);
                await _database.KeyDeleteAsync(typeKey);
            }

            try
            {
                var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

                await _database.StringSetAsync(typeKey, dto.UserType.ToString(), TimeSpan.FromMinutes(20));
                await _database.StringSetAsync(otpKey, otp, TimeSpan.FromMinutes(20));

             
                var message = $"[Crafty] Your OTP code is {otp}. Valid for 20 minutes.";
                var sent = await _SMS.SendAsync(dto.PhoneNumber, message);

                if (!sent.IsSuccess)
                {
                    await RevertRedisKeysAsync(); 
                    return Error.Failure("SMS.Failed", "فشل في إرسال الرسالة");
                }

                return Result.Ok();
            }
            catch (Exception)
            {
                await RevertRedisKeysAsync();
                return Error.Failure("System.Error", "حدث خطأ غير متوقع أثناء إرسال الكود");
            }
        }

        public async Task<Result<AuthResultDto>> VerifyOtpAsync(VerifyOtpDto dto)
        {
            // 1. Fail-Fast Validations (Input)
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.OtpCode))
                return Error.Validation("OTP.InvalidInput", "بيانات غير صحيحة");

            var otpKey = $"otp:{dto.PhoneNumber}";
            var typeKey = $"type:{dto.PhoneNumber}";

            var storedOtp = await _database.StringGetAsync(otpKey);
            if (storedOtp.IsNullOrEmpty)
                return Error.NotFound("OTP.Expired", "انتهت صلاحية الكود");

            if (storedOtp != dto.OtpCode)
                return Error.InvalidCrendentials("OTP.Wrong", "الكود غير صحيح");

            var userType = await _database.StringGetAsync(typeKey);

            await _database.KeyDeleteAsync(otpKey);
            await _database.KeyDeleteAsync(typeKey);

            bool isNew = false;
            async Task RevertUserCreationAsync(ApplicationUser createdUser)
            {
                if (createdUser != null)
                    await userManager.DeleteAsync(createdUser);
            }

            try
            {
              
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = dto.PhoneNumber,
                        PhoneNumber = dto.PhoneNumber
                    };

                    var createResult = await userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                        return Error.Failure("User.CreateFailed", "فشل إنشاء المستخدم");

                    var roleResult = await userManager.AddToRoleAsync(user, userType.ToString());
                    if (!roleResult.Succeeded)
                    {
                        await RevertUserCreationAsync(user); 
                        return Error.Failure("User.RoleFailed", "فشل تعيين صلاحية المستخدم");
                    }
                    isNew= true;
                }

                // 5. إنشاء التوكن وإرجاع النتيجة
                var token = await CreateTokenAsync(user);

                return Result<AuthResultDto>.Ok(new AuthResultDto
                {
                    UserId = user.Id,
                    IsAuthenticated = true,
                    Token = token,
                    PhoneNumber = dto.PhoneNumber,
                    IsNew = isNew,
                    ExpiresOn = DateTime.UtcNow.AddMonths(1)
                });
            }
            catch (Exception)
            {
                return Error.Failure("System.Error", "حدث خطأ أثناء معالجة بيانات المستخدم");
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new("TokenId", Guid.NewGuid().ToString()),
            new(ClaimTypes.MobilePhone, user.PhoneNumber!)
        };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTOptions")["issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1), // 💡 الأفضل دائماً استخدام UtcNow مع التوكنز
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
