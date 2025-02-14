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

        // Informações do usuário que criou o post
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        // Estatísticas do post
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }

        // Imagens associadas ao post
        public IEnumerable<PostImageDto>? Images { get; set; } = new List<PostImageDto>();

        // Lista de comentários do post
        public IEnumerable<CommentDto>? Comments { get; set; } = new List<CommentDto>();
    }
}
