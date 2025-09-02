using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IFollowerRepository
    {
        Task<IEnumerable<long>> GetFollowedUserIds(long followerId);

        Task<IEnumerable<Follower>> GetFollowersAsync(long userId, int pageNumber, int pageSize);

        Task<IEnumerable<Follower>> GetFollowingAsync(long userId, int pageNumber, int pageSize);

        Task<int> GetFollowersCountAsync(long userId);

        Task<int> GetFollowingCountAsync(long userId);

        Task<Follower> FollowUserAsync(long followerId, long userId);

        Task<bool> UnfollowUserAsync(long followerId, long userId);

        Task<bool> IsFollowingAsync(long followerId, long userId);
    }
}
