using CleanSharpArchitecture.Application.DTOs.Followers;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IFollowerService
    {
        Task<IEnumerable<FollowerDto>> GetFollowersAsync(long userId, int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<FollowingDto>> GetFollowingAsync(long userId, int pageNumber = 1, int pageSize = 10);
        Task<int> GetFollowersCountAsync(long userId);
        Task<int> GetFollowingCountAsync(long userId);
        Task<bool> FollowUserAsync(long followerId, long userId);
        Task<bool> UnfollowUserAsync(long followerId, long userId);
    }
}