using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;

namespace CleanSharpArchitecture.Application.DTOs.Users.Responses
{
    public class UserProfileDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public string ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Profession { get; set; }
        public string? ProfileMusic { get; set; }
        public DateTime? BirthDate { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
        public List<UserPhotoDto>? Photos { get; set; }
    }
}