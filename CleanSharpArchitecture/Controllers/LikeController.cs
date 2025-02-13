using CleanSharpArchitecture.Application.DTOs.Likes.Request;
using CleanSharpArchitecture.Application.DTOs.Likes.Response;
using CleanSharpArchitecture.Application.DTOs.Likes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Application.Services.Interfaces;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        /// <summary>
        /// Cria um novo like.
        /// </summary>
        /// <param name="createLikeDto">DTO contendo o UserId e o PostId.</param>
        /// <returns>Retorna o resultado da operação de criação.</returns>
        [HttpPost]
        public async Task<LikeResultDto> CreateLike([FromBody] CreateLikeDto createLikeDto)
        {
            return await _likeService.CreateLike(createLikeDto);
        }

        /// <summary>
        /// Exclui um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like a ser excluído.</param>
        /// <returns>Retorna o resultado da operação de exclusão.</returns>
        [HttpDelete("{id}")]
        public async Task<LikeResultDto> DeleteLike(Guid id)
        {
            return await _likeService.DeleteLike(id);
        }

        /// <summary>
        /// Recupera um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like.</param>
        /// <returns>id os dados do like ou null se não encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<LikeDto?> GetLikeById(Guid id)
        {
            return await _likeService.GetLikeById(id);
        }

        /// <summary>
        /// Recupera todos os likes paginados, aplicando filtros opcionais por PostId e EntityStatus.
        /// </summary>
        /// <param name="postId">Opcional: se fornecido, retorna apenas os likes do post especificado.</param>
        /// <param name="status">Opcional: se fornecido, retorna apenas os likes com o status especificado.</param>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Quantidade de likes por página (padrão 10).</param>
        /// <returns>Retorna uma coleção de <see cref="LikeDto"/>.</returns>
        [HttpGet]
        public async Task<IEnumerable<LikeDto>> GetAllLikes([FromQuery] Guid? postId, [FromQuery] EntityStatus? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _likeService.GetAllLikes(postId, status, pageNumber, pageSize);
        }
    }
}
