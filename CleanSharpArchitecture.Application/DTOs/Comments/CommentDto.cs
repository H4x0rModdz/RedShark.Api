namespace CleanSharpArchitecture.Application.DTOs.Comments
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
