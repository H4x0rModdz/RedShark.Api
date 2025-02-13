using CleanSharpArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Auth.Request
{
    public class RegisterDto
    {
        public string UserName { get; set; } // Ref.: @User7667
        public string Name { get; set; } // Ref.: User
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Biography { get; set; }
        public List<UserInterest> Interests { get; set; }
    }
}
