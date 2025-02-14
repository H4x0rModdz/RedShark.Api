namespace CleanSharpArchitecture.Application.DTOs.Posts.Response
{
    public class GetPostDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public List<string>? ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }
    }
}
