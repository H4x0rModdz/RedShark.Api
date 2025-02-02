using CleanSharpArchitecture.Application.Repositories.Interfaces;
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

        /// <summary>
        /// Cria um novo usuário no repositório.
        /// </summary>
        /// <param name="user">O usuário a ser criado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário criado.</returns>
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

        /// <summary>
        /// Seleciona um usuário pelo seu e-mail.
        /// </summary>
        /// <param name="email">O e-mail do usuário a ser buscado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário encontrado ou null.</returns>
        public async Task<User> SelectByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Seleciona um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do usuário a ser buscado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário encontrado ou null.</returns>
        public async Task<User> SelectById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Atualiza as informações de um usuário existente.
        /// </summary>
        /// <param name="user">O usuário com as informações atualizadas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um usuário do repositório.
        /// </summary>
        /// <param name="id">O ID do usuário a ser removido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Seleciona um usuário pelo seu código de recuperação de senha.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task<User> SelectByRecoveryCode(string code)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RecoveryCode == code);
        }
    }
}