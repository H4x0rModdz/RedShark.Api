namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}
