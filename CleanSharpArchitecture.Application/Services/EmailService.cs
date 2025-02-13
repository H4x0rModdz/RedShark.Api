using CleanSharpArchitecture.Application.Services;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailService(IOptions<SmtpSettings> smtpSettings) => _smtpSettings = smtpSettings.Value;

        public async Task SendEmail(string to, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(_smtpSettings.Host))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
                smtpClient.Port = _smtpSettings.Port;
                smtpClient.EnableSsl = _smtpSettings.EnableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
