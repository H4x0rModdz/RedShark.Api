using CleanSharpArchitecture.Application.DTOs.Feeds.Requests;
using CleanSharpArchitecture.Application.DTOs.Feeds.Responses;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IFeedService
    {
        Task<FeedResponseDto> GetFeedAsync(FeedRequestDto feedRequestDto);
    }
}
