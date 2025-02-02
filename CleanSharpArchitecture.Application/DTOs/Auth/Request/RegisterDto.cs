using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Auth.Request
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ConfirmEmail { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Biography { get; set; }
    }
}
