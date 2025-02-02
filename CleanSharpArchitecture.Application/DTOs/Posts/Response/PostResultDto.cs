namespace CleanSharpArchitecture.Application.DTOs.Posts.Response
{
    public class PostResultDto
    {
        public bool Success { get; set; }
        public string? PostId { get; set; }
        public List<string> Errors { get; set; }
    }
}
