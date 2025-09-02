namespace CleanSharpArchitecture.Application.DTOs.Feeds.Requests
{
    public class FeedRequestDto
    {
        public long? UserId { get; set; }

        public string? Cursor { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
