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

        public async Task<LikeResultDto> DeleteLike(long id)
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

        public async Task<LikeDto?> GetLikeById(long id)
        {
            var like = await _likeRepository.GetById(id);
            return like == null ? null : _mapper.Map<LikeDto>(like);
        }
        public async Task<IEnumerable<LikeDto>> GetAllLikes(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10)
        {
            var likes = await _likeRepository.GetAll(postId, status, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<LikeDto?> GetLikeByUserAndPost(long userId, long postId)
        {
            var like = await _likeRepository.GetByUserAndPost(userId, postId);
            return like == null ? null : _mapper.Map<LikeDto>(like);
        }

        public async Task<LikeResultDto> ToggleLike(long userId, long postId)
        {
            try
            {
                Log.Information($"ToggleLike called with UserId: {userId}, PostId: {postId}");
                
                var existingLike = await _likeRepository.GetByUserAndPost(userId, postId);
                
                if (existingLike != null)
                {
                    await _likeRepository.Delete(existingLike.Id);
                    var likesCount = await _likeRepository.GetLikesCountByPostId(postId);
                    Log.Information($"Like {existingLike.Id} removed successfully.");
                    return new LikeResultDto
                    {
                        Success = true,
                        IsLiked = false,
                        LikesCount = likesCount,
                        Errors = new List<string>()
                    };
                }
                else
                {
                    var newLike = new Like
                    {
                        UserId = userId,
                        PostId = postId,
                        Status = EntityStatus.Active
                    };

                    var createdLike = await _likeRepository.Create(newLike);
                    var likesCount = await _likeRepository.GetLikesCountByPostId(postId);
                    Log.Information($"Like {createdLike.Id} created successfully.");
                    return new LikeResultDto
                    {
                        Success = true,
                        LikeId = createdLike.Id,
                        IsLiked = true,
                        LikesCount = likesCount,
                        Errors = new List<string>()
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error toggling like: {ex.Message}");
                return new LikeResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<LikeResultDto> ToggleCommentLike(long userId, long commentId)
        {
            try
            {
                Log.Information($"ToggleCommentLike called with UserId: {userId}, CommentId: {commentId}");
                var existingLike = await _likeRepository.GetByUserAndComment(userId, commentId);
                
                if (existingLike != null)
                {
                    await _likeRepository.Delete(existingLike.Id);
                    var likesCount = await _likeRepository.GetLikesCountByCommentId(commentId);
                    Log.Information($"Comment like {existingLike.Id} removed successfully.");
                    return new LikeResultDto
                    {
                        Success = true,
                        IsLiked = false,
                        LikesCount = likesCount,
                        Errors = new List<string>()
                    };
                }
                else
                {
                    var newLike = new Like
                    {
                        UserId = userId,
                        CommentId = commentId,
                        Status = EntityStatus.Active
                    };

                    var createdLike = await _likeRepository.Create(newLike);
                    var likesCount = await _likeRepository.GetLikesCountByCommentId(commentId);
                    Log.Information($"Comment like {createdLike.Id} created successfully.");
                    return new LikeResultDto
                    {
                        Success = true,
                        LikeId = createdLike.Id,
                        IsLiked = true,
                        LikesCount = likesCount,
                        Errors = new List<string>()
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error toggling comment like: {ex.Message}");
                return new LikeResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
