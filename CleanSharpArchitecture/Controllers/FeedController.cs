using CleanSharpArchitecture.Application.DTOs.Feeds.Requests;
using CleanSharpArchitecture.Application.DTOs.Feeds.Responses;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedController : ControllerBase
    {
        private readonly IFeedService _feedService;

        public FeedController(IFeedService feedService) => _feedService = feedService;

        [HttpGet] // GET: api/Feed
        public async Task<FeedResponseDto> GetFeed([FromQuery] FeedRequestDto feedRequestDto)
        {
            if (feedRequestDto.UserId is null)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User Id not found in token.");
                feedRequestDto.UserId = long.Parse(userIdClaim);
            }

            return await _feedService.GetFeedAsync(feedRequestDto);
        }
    }
}
