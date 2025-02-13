namespace CleanSharpArchitecture.Application.DTOs.Posts.Response
{
    public class GetPostDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public List<string>? ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
