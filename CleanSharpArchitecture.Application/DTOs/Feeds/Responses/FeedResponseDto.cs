using CleanSharpArchitecture.Application.DTOs.Posts.Request;

namespace CleanSharpArchitecture.Application.DTOs.Feeds.Responses
{
    /// <summary>
    /// DTO containing feed data, including the list of posts and pagination information.
    /// </summary>
    public class FeedResponseDto
    {
        /// <summary>
        /// List of posts that make up the feed.
        /// </summary>
        public IEnumerable<PostDto> Posts { get; set; } = new List<PostDto>();

        /// <summary>
        /// Cursor for the next page of results. Can be null if there are no more items.
        /// </summary>
        public string? NextCursor { get; set; }
    }
}
