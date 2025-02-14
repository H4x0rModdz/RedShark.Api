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
    }
}
