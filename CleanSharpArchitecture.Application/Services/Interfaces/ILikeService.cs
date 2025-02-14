using CleanSharpArchitecture.Application.DTOs.Likes;
using CleanSharpArchitecture.Application.DTOs.Likes.Request;
using CleanSharpArchitecture.Application.DTOs.Likes.Response;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ILikeService
    {
        /// <summary>
        /// Cria um novo like para um post.
        /// </summary>
        /// <param name="createLikeDto">DTO contendo o UserId e o PostId.</param>
        /// <returns>Retorna o resultado da operação.</returns>
        Task<LikeResultDto> CreateLike(CreateLikeDto createLikeDto);

        /// <summary>
        /// Exclui um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like a ser removido.</param>
        /// <returns>Retorna o resultado da operação.</returns>
        Task<LikeResultDto> DeleteLike(long id);

        /// <summary>
        /// Recupera um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like.</param>
        /// <returns>Retorna o DTO do like ou null se não encontrado.</returns>
        Task<LikeDto?> GetLikeById(long id);

        /// <summary>
        /// Recupera todos os likes, opcionalmente filtrando por PostId.
        /// </summary>
        /// <param name="postId">Opcional: se fornecido, retorna apenas os likes do post especificado.</param>
        /// <returns>Retorna uma coleção de LikeDto.</returns>
        Task<IEnumerable<LikeDto>> GetAllLikes(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10);
    }
}
