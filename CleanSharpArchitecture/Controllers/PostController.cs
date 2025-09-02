using CleanSharpArchitecture.API.Attributes;
using CleanSharpArchitecture.API.Extensions;
using CleanSharpArchitecture.Application.DTOs.Feeds.Requests;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService) 
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        /// <summary>
        /// Creates a new post with optional images
        /// </summary>
        /// <param name="postDto">Post creation data</param>
        /// <returns>Post creation result</returns>
        /// <response code="200">Post created successfully</response>
        /// <response code="400">Invalid post data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="429">Too many posts created</response>
        [HttpPost]
        [RequireAuthentication]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(PostResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<PostResultDto>> CreatePost([FromForm, Required] CreatePostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var userId = this.GetCurrentUserId();
            postDto.UserId = userId;

            var result = await _postService.CreatePost(postDto);
            
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Updates an existing post
        /// </summary>
        /// <param name="postDto">Post update data</param>
        /// <returns>Post update result</returns>
        /// <response code="200">Post updated successfully</response>
        /// <response code="400">Invalid post data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Post not found</response>
        [HttpPatch]
        [RequirePostOwnership("PostId")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(PostResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PostResultDto>> UpdatePost([FromForm, Required] UpdatePostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var currentUserId = this.GetCurrentUserId();
            var result = await _postService.UpdatePost(postDto, currentUserId);
            
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Deletes a post by ID
        /// </summary>
        /// <param name="id">Post ID to delete</param>
        /// <returns>Post deletion result</returns>
        /// <response code="200">Post deleted successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Post not found</response>
        [HttpDelete("{id}")]
        [RequirePostOwnership("id")]
        [ProducesResponseType(typeof(PostResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PostResultDto>> DeletePost([Required] long id)
        {
            var currentUserId = this.GetCurrentUserId();
            var result = await _postService.DeletePost(id, currentUserId);
            
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Retrieves paginated list of posts
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <returns>List of posts</returns>
        /// <response code="200">Posts retrieved successfully</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPostDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetAllPosts(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1, 
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            var result = await _postService.GetAllPosts(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific post by ID
        /// </summary>
        /// <param name="id">Post ID</param>
        /// <returns>Post details</returns>
        /// <response code="200">Post found</response>
        /// <response code="400">Invalid post ID</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Post not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetPostDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetPostDto>> GetPostById([Required] long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid post ID.");
            }

            var result = await _postService.GetPostById(id);
            
            if (result == null)
            {
                return NotFound("Post not found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated posts by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <returns>List of user posts</returns>
        /// <response code="200">Posts retrieved successfully</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("user/{userId}")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<PostDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByUserId(
            [Required] long userId,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            var posts = await _postService.GetPostsByUserId(userId, pageNumber, pageSize);
            return Ok(posts);
        }

        /// <summary>
        /// Retrieves the count of posts by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Count of user posts</returns>
        /// <response code="200">Count retrieved successfully</response>
        /// <response code="400">Invalid user ID</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("user/{userId}/count")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<int>> GetPostsCountByUserId([Required] long userId)
        {
            var count = await _postService.GetPostsCountByUserId(userId);
            return Ok(count);
        }
    }
}
