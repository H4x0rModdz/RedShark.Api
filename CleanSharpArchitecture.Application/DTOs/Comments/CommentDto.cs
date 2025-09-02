namespace CleanSharpArchitecture.Application.DTOs.Comments
{
    public class CommentDto
    {
        public long Id { get; set; }

        // Information about the user who made the comment
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        // Content and statistics of the comment
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
