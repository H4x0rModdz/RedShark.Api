using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class CreatePostDto
    {
        public string Content { get; set; }
        public List<IFormFile>? Images { get; set; }
        public Guid UserId { get; set; }
    }
}
