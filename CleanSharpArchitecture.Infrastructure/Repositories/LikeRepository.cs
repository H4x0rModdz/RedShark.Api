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

        public async Task Delete(Guid likeId)
        {
            var like = await GetById(likeId);
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Like?> GetById(Guid likeId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == likeId);
        }

        /// <summary>
        /// Recupera todos os likes paginados, aplicando filtros opcionais por PostId e EntityStatus.
        /// </summary>
        /// <param name="postId">
        /// Opcional: se fornecido, retorna apenas os likes associados ao post especificado.
        /// </param>
        /// <param name="status">
        /// Opcional: se fornecido, retorna apenas os likes com o status especificado.
        /// </param>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Quantidade de likes por página (padrão 10).</param>
        /// <returns>Retorna uma coleção paginada de likes.</returns>
        public async Task<IEnumerable<Like>> GetAll(Guid? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10)
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
    }
}
