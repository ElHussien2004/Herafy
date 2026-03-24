using ServiceAbstraction;
using Shared.DTOs.UserDTOS;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthService(IConnectionMultiplexer connection,ISMSService _SMS) : IAuthService
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<bool> SendOtpAsync(string phoneNumber)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var key = $"{phoneNumber}";

            //store in redis
            await _database.StringSetAsync(key, otp,TimeSpan.FromMinutes(10));
            var Massage = "TODo";
           await  _SMS.SendAsync(phoneNumber, Massage);
           return true;
        }

        public Task<AuthResultDto> VerifyOtpAsync(string phoneNumber, string otpCode)
        {
            throw new NotImplementedException();
        }
    }
}
