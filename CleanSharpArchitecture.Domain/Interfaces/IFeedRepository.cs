using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IFeedRepository
    {
        Task<List<Post>> GetPostsForFeed(IEnumerable<Guid> followedUserIds, string? cursor, int pageSize);
    }
}
