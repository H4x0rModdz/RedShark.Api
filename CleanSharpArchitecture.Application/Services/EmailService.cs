using CleanSharpArchitecture.Application.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string to, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.seuservidor.com"))
            {
                smtpClient.Credentials = new NetworkCredential("seu-email@dominio.com", "sua-senha");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("seu-email@dominio.com"),
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
