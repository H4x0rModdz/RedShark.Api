using CleanSharpArchitecture.Application.DTOs.Posts.PostImages;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public IEnumerable<PostImageDto>? Images { get; set; } = new List<PostImageDto>();
    }
}
