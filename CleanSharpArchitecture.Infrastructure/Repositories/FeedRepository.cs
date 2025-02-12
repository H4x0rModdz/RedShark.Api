using CleanSharpArchitecture.Domain.Entities.Posts;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsForFeed(IEnumerable<Guid> followedUserIds, string? cursor, int pageSize)
        {
            var query = _context.Posts.AsQueryable();

            query = query.Where(p => followedUserIds.Contains(p.UserId));

            if (!string.IsNullOrEmpty(cursor))
            {
                DateTime cursorDate = DateTime.Parse(cursor, null, DateTimeStyles.RoundtripKind);
                query = query.Where(p => p.CreatedAt < cursorDate);
            }

            query = query.OrderByDescending(p => p.CreatedAt)
                         .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
