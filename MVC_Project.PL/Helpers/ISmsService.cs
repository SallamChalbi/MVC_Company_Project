using MVC_Project.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace MVC_Project.PL.Helpers
{
    public interface ISmsService
    {
        public MessageResource SendSms(SmsMessage sms);
    }
}
