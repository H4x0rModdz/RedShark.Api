using CleanSharpArchitecture.Application.DTOs.Feeds.Requests;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService) => _postService = postService;

        [HttpPost] // POST: api/Post
        [Consumes("multipart/form-data")]
        public async Task<PostResultDto> CreatePost([FromForm] CreatePostDto postDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User Id not found in token.");
            postDto.UserId = long.Parse(userIdClaim);

            return await _postService.CreatePost(postDto);
        }

        [HttpPut] // PUT: api/Post
        [Consumes("multipart/form-data")]
        public async Task<PostResultDto> UpdatePost([FromForm] UpdatePostDto postDto)
        {
            return await _postService.UpdatePost(postDto);
        }

        [HttpDelete("{id}")] // DELETE: api/Post/{id}
        public async Task<PostResultDto> DeletePost(long id)
        {
            return await _postService.DeletePost(id);
        }

        [HttpGet] // GET: api/Post
        public async Task<IEnumerable<GetPostDto>> GetAllPosts([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            return await _postService.GetAllPosts(pageNumber, pageSize);
        }

        [HttpGet("{id}")] // GET: api/Post/{id}
        public async Task<GetPostDto?> GetPostById(long id)
        {
            return await _postService.GetPostById(id);
        }
    }
}
