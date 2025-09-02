using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class FollowerRepository : IFollowerRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<long>> GetFollowedUserIds(long followerId)
        {
            return await _context.Followers
                .Where(f => f.FollowerId == followerId)
                .Select(f => f.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Follower>> GetFollowersAsync(long userId, int pageNumber, int pageSize)
        {
            return await _context.Followers
                .Where(f => f.UserId == userId)
                .Include(f => f.FollowerUser)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Follower>> GetFollowingAsync(long userId, int pageNumber, int pageSize)
        {
            return await _context.Followers
                .Where(f => f.FollowerId == userId)
                .Include(f => f.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetFollowersCountAsync(long userId)
        {
            return await _context.Followers
                .CountAsync(f => f.UserId == userId);
        }

        public async Task<int> GetFollowingCountAsync(long userId)
        {
            return await _context.Followers
                .CountAsync(f => f.FollowerId == userId);
        }

        public async Task<Follower> FollowUserAsync(long followerId, long userId)
        {
            var follower = new Follower
            {
                FollowerId = followerId,
                UserId = userId
            };

            _context.Followers.Add(follower);
            await _context.SaveChangesAsync();
            
            return follower;
        }

        public async Task<bool> UnfollowUserAsync(long followerId, long userId)
        {
            var follower = await _context.Followers
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.UserId == userId);

            if (follower == null)
                return false;

            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> IsFollowingAsync(long followerId, long userId)
        {
            return await _context.Followers
                .AnyAsync(f => f.FollowerId == followerId && f.UserId == userId);
        }
    }
}
