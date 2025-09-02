using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Like> Create(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task Delete(long likeId)
        {
            var like = await GetById(likeId);
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Like?> GetById(long likeId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == likeId);
        }

        public async Task<IEnumerable<Like>> GetAll(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Likes
                .Include(l => l.User)
                .AsQueryable();

            if (postId.HasValue)
                query = query.Where(l => l.PostId == postId.Value);

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Like?> GetByUserAndPost(long userId, long postId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId && l.Status == EntityStatus.Active);
        }

        public async Task<int> GetLikesCountByPostId(long postId)
        {
            return await _context.Likes
                .Where(l => l.PostId == postId && l.Status == EntityStatus.Active)
                .CountAsync();
        }

        public async Task<Like?> GetByUserAndComment(long userId, long commentId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == commentId && l.Status == EntityStatus.Active);
        }

        public async Task<int> GetLikesCountByCommentId(long commentId)
        {
            return await _context.Likes
                .Where(l => l.CommentId == commentId && l.Status == EntityStatus.Active)
                .CountAsync();
        }
    }
}
