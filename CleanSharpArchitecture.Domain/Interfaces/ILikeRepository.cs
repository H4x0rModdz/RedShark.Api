using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface ILikeRepository
    {
        /// <summary>
        /// Cria um novo like no banco.
        /// </summary>
        /// <param name="like">Entidade like a ser criada.</param>
        /// <returns>Retorna o like criado.</returns>
        Task<Like> Create(Like like);

        /// <summary>
        /// Exclui um like pelo seu ID.
        /// </summary>
        /// <param name="likeId">ID do like a ser removido.</param>
        Task Delete(long likeId);

        /// <summary>
        /// Recupera um like pelo seu ID.
        /// </summary>
        /// <param name="likeId">ID do like.</param>
        /// <returns>Retorna o like ou null se não encontrado.</returns>
        Task<Like?> GetById(long likeId);

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
        Task<IEnumerable<Like>> GetAll(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10);
    }
}
