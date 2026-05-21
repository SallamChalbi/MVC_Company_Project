using Microsoft.Extensions.Options;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MVC_Project.PL.Helpers
{
    public class SmsService : ISmsService
    {
        private TwilioSettings _options;
        public SmsService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }

        public MessageResource SendSms(SmsMessage sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);
            var result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
                to: sms.PhoneNumber);

            return result;
        }
    }
}
