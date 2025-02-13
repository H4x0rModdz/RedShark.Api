namespace CleanSharpArchitecture.Application.DTOs.Auth.Response
{
    public class RegisterResultDto
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
