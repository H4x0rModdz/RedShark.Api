using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Feeds.Requests;
using CleanSharpArchitecture.Application.DTOs.Feeds.Responses;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.Interfaces.Repositories;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Interfaces;

namespace CleanSharpArchitecture.Application.Services
{
    public class FeedService : IFeedService
    {
        private readonly IPostRepository _postRepository;
        private readonly IFollowerRepository _followerRepository;
        private readonly IMapper _mapper;

        public FeedService(
            IPostRepository postRepository,
            IFollowerRepository followerRepository,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _followerRepository = followerRepository;
            _mapper = mapper;
        }

        public async Task<FeedResponseDto> GetFeedAsync(FeedRequestDto feedRequestDto)
        {
            var followedUserIds = await _followerRepository.GetFollowedUserIds(feedRequestDto.UserId.Value);

            var posts = await _postRepository.GetPostsForFeed(followedUserIds, feedRequestDto.Cursor, feedRequestDto.PageSize);

            var postDtos = _mapper.Map<List<PostDto>>(posts);
            
            var nextCursor = postDtos.Any()
                ? postDtos.Last().Id.ToString()
                : null;

            return new FeedResponseDto
            {
                Posts = postDtos,
                NextCursor = nextCursor
            };
        }
    }
}
