using CleanSharpArchitecture.Application.DTOs.Posts.PostImages;
using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class UpdatePostDto
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public IEnumerable<Guid>? ImagesToKeep { get; set; } = new List<Guid>();
        public IEnumerable<IFormFile>? NewImages { get; set; } = new List<IFormFile>();
        //public IEnumerable<PostImageDto>? Images { get; set; } = new List<PostImageDto>();
    }
}
