using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Domain.Dictionaries;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            if (status.HasValue)
                query = query.Where(u => u.Status == status.Value);

            if (include?.Contains("posts") == true)
                query = query.Include(u => u.Posts)
                             .ThenInclude(p => p.Images);

            query = ApplyIncludes(query, include);

            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        private IQueryable<User> ApplyIncludes(IQueryable<User> query, string? include)
        {
            if (string.IsNullOrEmpty(include))
                return query;

            var includes = include.Split(',');

            foreach (var inc in includes)
            {
                if (IncludeMap.UserIncludeDictionary.TryGetValue(inc.Trim().ToLower(), out var expression))
                    query = query.Include(expression);
            }

            return query;
        }

        public async Task<User> SelectByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> SelectById(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> SelectByUserName(string userName)
        {
            var query = _context.Users.AsQueryable();
                        
            return await query.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task Update(User user)
        {
            // Handle entity tracking to avoid conflicts
            var entry = _context.Entry(user);
            if (entry.State == EntityState.Detached)
            {
                _context.Users.Update(user);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> SelectByRecoveryCode(string code)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RecoveryCode == code);
        }
    }
}