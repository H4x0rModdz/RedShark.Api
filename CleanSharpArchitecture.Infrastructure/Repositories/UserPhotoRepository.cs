using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class UserPhotoRepository : IUserPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPhotoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserPhoto> Create(UserPhoto userPhoto)
        {
            await _context.UserPhotos.AddAsync(userPhoto);
            await _context.SaveChangesAsync();
            return userPhoto;
        }

        public async Task<UserPhoto?> GetById(long photoId)
        {
            return await _context.UserPhotos
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<IEnumerable<UserPhoto>> GetByUserId(long userId, int pageNumber = 1, int pageSize = 20)
        {
            return await _context.UserPhotos
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task Update(UserPhoto userPhoto)
        {
            _context.UserPhotos.Update(userPhoto);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long photoId)
        {
            var photo = await _context.UserPhotos.FindAsync(photoId);
            if (photo != null)
            {
                _context.UserPhotos.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserPhoto>> GetAll(long? userId = null, EntityStatus? status = null, int pageNumber = 1, int pageSize = 20)
        {
            var query = _context.UserPhotos.AsQueryable();

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId.Value);

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}