namespace CleanSharpArchitecture.Application.DTOs.Auth.Response
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public Guid Id { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
