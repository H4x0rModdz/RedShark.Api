using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Application.DTOs.Posts.PostImages;

namespace CleanSharpArchitecture.Application.DTOs.Posts.Request
{
    public class PostDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Information about the user who created the post
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        // Post statistics
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }

        // Images associated with the post
        public IEnumerable<PostImageDto>? Images { get; set; } = new List<PostImageDto>();

        // List of post comments
        public IEnumerable<CommentDto>? Comments { get; set; } = new List<CommentDto>();
    }
}
