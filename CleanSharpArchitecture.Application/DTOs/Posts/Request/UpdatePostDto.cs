using CleanSharpArchitecture.Application.DTOs.Posts.PostImages;
using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class UpdatePostDto
    {
        public long PostId { get; set; }
        public string Content { get; set; }
        public IEnumerable<long>? ImagesToKeep { get; set; } = new List<long>();
        public IEnumerable<IFormFile>? NewImages { get; set; } = new List<IFormFile>();
        //public IEnumerable<PostImageDto>? Images { get; set; } = new List<PostImageDto>();
    }
}
