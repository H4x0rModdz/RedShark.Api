namespace CleanSharpArchitecture.Application.DTOs.Auth.Response
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Biography { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
