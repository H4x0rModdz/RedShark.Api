using CleanSharpArchitecture.Application.DTOs.Likes;
using CleanSharpArchitecture.Application.DTOs.Likes.Request;
using CleanSharpArchitecture.Application.DTOs.Likes.Response;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ILikeService
    {
        Task<LikeResultDto> CreateLike(CreateLikeDto createLikeDto);

        Task<LikeResultDto> DeleteLike(long id);

        Task<LikeDto?> GetLikeById(long id);

        Task<IEnumerable<LikeDto>> GetAllLikes(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10);

        Task<LikeDto?> GetLikeByUserAndPost(long userId, long postId);

        Task<LikeResultDto> ToggleLike(long userId, long postId);

        Task<LikeResultDto> ToggleCommentLike(long userId, long commentId);
    }
}
