using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.CommonResult;
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
         public async Task<Result> SendAsync(string to, string message)
         {
             try
             {
                if (string.IsNullOrWhiteSpace(to))
                    return Error.Validation("SMS.ToRequired", "رقم الهاتف مطلوب");

                if (string.IsNullOrWhiteSpace(message))
                    return Error.Validation("SMS.MessageRequired", "محتوى الرسالة مطلوب");

                TwilioClient.Init(
                    _config["Twilio:AccountSid"],
                    _config["Twilio:AuthToken"]
                );

                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(_config["Twilio:PhoneNumber"]),
                    to: new PhoneNumber(to)
                );

                // check status
                if (result.Status == MessageResource.StatusEnum.Failed ||
                    result.Status == MessageResource.StatusEnum.Undelivered)
                {
                    return Error.Failure("SMS.Failed", "فشل في إرسال الرسالة");
                }

                return Result.Ok();
             }
             catch (Exception ex)
             {
                // ممكن تضيف logging هنا
                return Error.Failure("SMS.Exception", ex.Message);
             }
         }
    }
}

