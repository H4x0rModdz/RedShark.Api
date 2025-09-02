using CleanSharpArchitecture.API.Attributes;
using CleanSharpArchitecture.API.Extensions;
using CleanSharpArchitecture.Application.DTOs.Followers;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class FollowerController : ControllerBase
    {
        private readonly IFollowerService _followerService;

        public FollowerController(IFollowerService followerService)
        {
            _followerService = followerService ?? throw new ArgumentNullException(nameof(followerService));
        }

        /// <summary>
        /// Retrieves paginated list of followers for a specific user
        /// </summary>
        [HttpGet("{userId}/followers")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<FollowerDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FollowerDto>>> GetFollowers(
            [Required] long userId,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            var followers = await _followerService.GetFollowersAsync(userId, pageNumber, pageSize);
            return Ok(followers);
        }

        /// <summary>
        /// Retrieves paginated list of users that a specific user is following
        /// </summary>
        [HttpGet("{userId}/following")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<FollowingDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FollowingDto>>> GetFollowing(
            [Required] long userId,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            var following = await _followerService.GetFollowingAsync(userId, pageNumber, pageSize);
            return Ok(following);
        }

        /// <summary>
        /// Retrieves the count of followers for a specific user
        /// </summary>
        [HttpGet("{userId}/followers/count")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> GetFollowersCount([Required] long userId)
        {
            var count = await _followerService.GetFollowersCountAsync(userId);
            return Ok(count);
        }

        /// <summary>
        /// Retrieves the count of users that a specific user is following
        /// </summary>
        [HttpGet("{userId}/following/count")]
        [ValidateUser("userId")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> GetFollowingCount([Required] long userId)
        {
            var count = await _followerService.GetFollowingCountAsync(userId);
            return Ok(count);
        }

        /// <summary>
        /// Follow a user
        /// </summary>
        [HttpPost("{userId}/follow")]
        [RequireAuthentication]
        [ValidateUser("userId")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<bool>> FollowUser([Required] long userId)
        {
            var currentUserId = this.GetCurrentUserId();
            var result = await _followerService.FollowUserAsync(currentUserId, userId);
            return Ok(result);
        }

        /// <summary>
        /// Unfollow a user
        /// </summary>
        [HttpDelete("{userId}/unfollow")]
        [RequireAuthentication]
        [ValidateUser("userId")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<bool>> UnfollowUser([Required] long userId)
        {
            var currentUserId = this.GetCurrentUserId();
            var result = await _followerService.UnfollowUserAsync(currentUserId, userId);
            return Ok(result);
        }
    }
}