namespace CleanSharpArchitecture.Application.DTOs.Auth.Response
{
    public class RecoveryResultDto
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
