using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class CreatePostDto
    {
        public string Content { get; set; }
        public List<IFormFile>? Images { get; set; }
        public long UserId { get; set; }
    }
}
