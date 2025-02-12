using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Likes;
using CleanSharpArchitecture.Application.DTOs.Likes.Request;
using CleanSharpArchitecture.Application.DTOs.Likes.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using Serilog;

namespace CleanSharpArchitecture.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepository, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo like para um post.
        /// </summary>
        /// <param name="createLikeDto">DTO contendo o UserId e o PostId.</param>
        /// <returns>Retorna um objeto <see cref="LikeResultDto"/> com o resultado da operação.</returns>
        public async Task<LikeResultDto> CreateLike(CreateLikeDto createLikeDto)
        {
            try
            {
                var like = _mapper.Map<Like>(createLikeDto);

                var createdLike = await _likeRepository.Create(like);

                Log.Information($"Like {createdLike.Id} created successfully.");

                return new LikeResultDto
                {
                    Success = true,
                    LikeId = createdLike.Id,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating like: {ex.Message}");
                return new LikeResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Exclui um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like a ser removido.</param>
        /// <returns>Retorna um objeto <see cref="LikeResultDto"/> com o resultado da operação.</returns>
        public async Task<LikeResultDto> DeleteLike(Guid id)
        {
            try
            {
                await _likeRepository.Delete(id);
                Log.Information($"Like {id} deleted successfully.");
                return new LikeResultDto
                {
                    Success = true,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting like: {ex.Message}");
                return new LikeResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Recupera um like pelo seu ID.
        /// </summary>
        /// <param name="id">ID do like.</param>
        /// <returns>Retorna um <see cref="LikeDto"/> ou null se não encontrado.</returns>
        public async Task<LikeDto?> GetLikeById(Guid id)
        {
            var like = await _likeRepository.GetById(id);
            return like == null ? null : _mapper.Map<LikeDto>(id);
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
        /// <returns>Retorna uma coleção de <see cref="LikeDto"/>.</returns>
        public async Task<IEnumerable<LikeDto>> GetAllLikes(Guid? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10)
        {
            var likes = await _likeRepository.GetAll(postId, status, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }
    }
}
