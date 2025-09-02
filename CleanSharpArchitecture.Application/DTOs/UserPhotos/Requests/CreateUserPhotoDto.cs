using System.ComponentModel.DataAnnotations;

namespace CleanSharpArchitecture.Application.DTOs.UserPhotos.Requests
{
    public class CreateUserPhotoDto
    {
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}