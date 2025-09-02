using CleanSharpArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Users.Requests
{
    public class UpdateUserDto
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Biography { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? CoverImage { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
