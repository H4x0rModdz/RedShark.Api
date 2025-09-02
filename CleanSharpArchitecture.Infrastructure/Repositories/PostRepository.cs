namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    using global::CleanSharpArchitecture.Application.Interfaces.Repositories;
    using global::CleanSharpArchitecture.Domain.Entities.Posts;
    using global::CleanSharpArchitecture.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    namespace CleanSharpArchitecture.Infrastructure.Data.Repositories
    {
        public class PostRepository : IPostRepository
        {
            private readonly ApplicationDbContext _context;

            public PostRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Post> Create(Post post)
            {
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                return post;
            }

            public async Task<Post?> GetById(long id)
            {
                return await _context.Posts
                    .Include(p => p.Images)
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                            .ThenInclude(r => r.User)
                    .Include(p => p.Likes)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }

            public async Task<IEnumerable<Post>> GetAll(int pageNumber = 1, int pageSize = 10)
            {
                var query = _context.Posts
                    .Include(p => p.Images)
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                            .ThenInclude(r => r.User)
                    .Include(p => p.Likes)
                    .AsQueryable();

                query = query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

                return await query.ToListAsync();
            }

            public async Task Update(Post post)
            {
                // Handle entity tracking to avoid conflicts in complex update scenarios
                var entry = _context.Entry(post);
                if (entry.State == EntityState.Detached)
                {
                    _context.Posts.Update(post);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }

            public async Task Delete(long id)
            {
                var post = await GetById(id);
                if (post != null)
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<IEnumerable<PostImage>> GetImagesByPostId(long postId)
            {
                return await _context.PostImages
                    .Where(pi => pi.PostId == postId)
                    .ToListAsync();
            }

            public async Task AddImages(IEnumerable<PostImage> images)
            {
                await _context.PostImages.AddRangeAsync(images);
                await _context.SaveChangesAsync();
            }

            public async Task RemoveImages(IEnumerable<PostImage> images)
            {
                _context.PostImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }

            public async Task<IEnumerable<Post>> GetPostsForFeed(IEnumerable<long> followedUserIds, string? cursor, int pageSize)
            {
                var query = _context.Posts
                    .Include(p => p.Images)
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                    .AsQueryable();

                // Cursor-based pagination for better performance with large datasets
                if (!string.IsNullOrEmpty(cursor))
                {
                    long cursorId = long.Parse(cursor);
                    query = query.Where(p => p.Id < cursorId);
                }

                // Prioritize followed users posts while maintaining chronological order
                query = query.OrderByDescending(p => followedUserIds.Contains(p.UserId) ? 1 : 0)
                             .ThenByDescending(p => p.Id)
                             .Take(pageSize);

                return await query.ToListAsync();
            }

            public async Task<IEnumerable<Post>> GetPostsByUserId(long userId, int pageNumber, int pageSize)
            {
                return await _context.Posts
                    .Include(p => p.Images)
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                            .ThenInclude(r => r.User)
                    .Include(p => p.Likes)
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }

            public async Task<int> GetPostsCountByUserId(long userId)
            {
                return await _context.Posts
                    .Where(p => p.UserId == userId)
                    .CountAsync();
            }
        }
    }
}
