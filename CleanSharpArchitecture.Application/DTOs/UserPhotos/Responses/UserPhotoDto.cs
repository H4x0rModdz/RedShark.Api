namespace CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses
{
    public class UserPhotoDto
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}