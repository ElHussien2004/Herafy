using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Service
{
    public class SMSService(IConfiguration _config) : ISMSService
    {
        public async Task SendAsync(string to, string message)
        {
            TwilioClient.Init(
            _config["Twilio:AccountSid"],
            _config["Twilio:AuthToken"]);

            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_config["Twilio:PhoneNumber"]),
                to: new PhoneNumber(to)
            );
        }
    }
}
