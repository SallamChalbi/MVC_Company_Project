using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Settings;

namespace MVC_Project.PL.Helpers
{
    public class EmailSettings: IEmailSettings
    {
        private readonly MailSettings _options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email!),
                Subject = email.Subject!
            };
            mail.To.Add(MailboxAddress.Parse(email.Recipient!));
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email!));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host!, _options.Port!, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email!, _options.Password!);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}
