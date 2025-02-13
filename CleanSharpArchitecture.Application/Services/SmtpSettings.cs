namespace CleanSharpArchitecture.Application.Services
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public bool EnableSsl { get; set; }
    }
}
