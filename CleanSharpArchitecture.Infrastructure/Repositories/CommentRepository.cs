using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Include(c => c.Replies)
                .FirstAsync(c => c.Id == comment.Id);
        }

        public async Task<Comment?> GetById(long commentId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == commentId && c.Status == EntityStatus.Active);
        }

        public async Task<IEnumerable<Comment>> GetByPostId(long postId, int pageNumber = 1, int pageSize = 20)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .Where(c => c.PostId == postId && c.ParentCommentId == null && c.Status == EntityStatus.Active)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetReplies(long commentId, int pageNumber = 1, int pageSize = 20)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Where(c => c.ParentCommentId == commentId && c.Status == EntityStatus.Active)
                .OrderBy(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task Update(Comment comment)
        {
            var entry = _context.Entry(comment);
            if (entry.State == EntityState.Detached)
            {
                _context.Comments.Update(comment);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long commentId)
        {
            var comment = await GetById(commentId);
            if (comment != null)
            {
                comment.Status = EntityStatus.Deleted;
                await Update(comment);
            }
        }

        public async Task<int> GetCommentCount(long postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId && c.Status == EntityStatus.Active)
                .CountAsync();
        }

        public async Task<IEnumerable<Comment>> GetAll(long? postId, long? userId, EntityStatus? status, int pageNumber = 1, int pageSize = 20)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Include(c => c.Replies)
                .AsQueryable();

            if (postId.HasValue)
                query = query.Where(c => c.PostId == postId.Value);

            if (userId.HasValue)
                query = query.Where(c => c.UserId == userId.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);
            else
                query = query.Where(c => c.Status == EntityStatus.Active);

            return await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}