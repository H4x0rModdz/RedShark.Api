using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var comment = await _commentService.CreateComment(request.PostId, userId, request.Content, request.ParentCommentId);
                return Ok(comment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<CommentDto>> GetCommentById(long commentId)
        {
            var comment = await _commentService.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPost(
            long postId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var comments = await _commentService.GetCommentsByPost(postId, pageNumber, pageSize);
            return Ok(comments);
        }

        [HttpGet("{commentId}/replies")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentReplies(
            long commentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var replies = await _commentService.GetCommentReplies(commentId, pageNumber, pageSize);
            return Ok(replies);
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(long commentId, [FromBody] UpdateCommentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var comment = await _commentService.UpdateComment(commentId, userId, request.Content);
                if (comment == null)
                {
                    return NotFound();
                }

                return Ok(comment);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(long commentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var success = await _commentService.DeleteComment(commentId, userId);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllComments(
            [FromQuery] long? postId,
            [FromQuery] long? userId,
            [FromQuery] EntityStatus? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var comments = await _commentService.GetAllComments(postId, userId, status, pageNumber, pageSize);
            return Ok(comments);
        }
    }

    public class CreateCommentRequest
    {
        public long PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? ParentCommentId { get; set; }
    }

    public class UpdateCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}