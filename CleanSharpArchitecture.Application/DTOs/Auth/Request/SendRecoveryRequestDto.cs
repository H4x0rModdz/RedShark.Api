namespace CleanSharpArchitecture.Application.DTOs.Auth.Request
{
    public class SendRecoveryRequestDto
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
