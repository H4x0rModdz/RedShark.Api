namespace CleanSharpArchitecture.Application.DTOs.Comments
{
    public class CommentDto
    {
        public long Id { get; set; }

        // Informações do usuário que fez o comentário
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        // Conteúdo e estatísticas do comentário
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
