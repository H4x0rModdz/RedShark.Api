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

        [HttpPost]
        public async Task<LikeResultDto> CreateLike([FromBody] CreateLikeDto createLikeDto)
        {
            return await _likeService.CreateLike(createLikeDto);
        }

        [HttpDelete("{id}")]
        public async Task<LikeResultDto> DeleteLike(long id)
        {
            return await _likeService.DeleteLike(id);
        }

        [HttpGet("{id}")]
        public async Task<LikeDto?> GetLikeById(long id)
        {
            return await _likeService.GetLikeById(id);
        }

        [HttpGet]
        public async Task<IEnumerable<LikeDto>> GetAllLikes([FromQuery] long? postId, [FromQuery] EntityStatus? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _likeService.GetAllLikes(postId, status, pageNumber, pageSize);
        }

        [HttpGet("user/{userId}/post/{postId}")]
        public async Task<LikeDto?> GetLikeByUserAndPost(long userId, long postId)
        {
            return await _likeService.GetLikeByUserAndPost(userId, postId);
        }

        [HttpPost("toggle")]
        public async Task<LikeResultDto> ToggleLike([FromBody] CreateLikeDto toggleLikeDto)
        {
            // Log the incoming request with detailed information
            Console.WriteLine($"ToggleLike request - UserId: {toggleLikeDto.UserId}, PostId: {toggleLikeDto.PostId}, CommentId: {toggleLikeDto.CommentId}");
            Console.WriteLine($"UserId type: {toggleLikeDto.UserId.GetType()}, Value as string: '{toggleLikeDto.UserId}'");
            
            if (toggleLikeDto.PostId.HasValue)
            {
                return await _likeService.ToggleLike(toggleLikeDto.UserId, toggleLikeDto.PostId.Value);
            }
            else if (toggleLikeDto.CommentId.HasValue)
            {
                return await _likeService.ToggleCommentLike(toggleLikeDto.UserId, toggleLikeDto.CommentId.Value);
            }
            else
            {
                return new LikeResultDto
                {
                    Success = false,
                    Errors = new List<string> { "PostId ou CommentId deve ser fornecido." }
                };
            }
        }
    }
}
