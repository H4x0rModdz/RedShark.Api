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
            // 1. Obter a lista de IDs dos usuários que o usuário autenticado está seguindo.
            var followedUserIds = await _followerRepository.GetFollowedUserIds(feedRequestDto.UserId.Value);

            // 2. Buscar os posts dos usuários seguidos, aplicando paginação baseada no cursor e no tamanho da página.
            var posts = await _postRepository.GetPostsForFeed(followedUserIds, feedRequestDto.Cursor, feedRequestDto.PageSize);

            // 3. Mapear os posts para o DTO de saída usando AutoMapper.
            var postDtos = _mapper.Map<List<PostDto>>(posts);
            
            // 4. Definir o cursor para a próxima página pelo Id.
            var nextCursor = postDtos.Any()
                ? postDtos.Last().Id.ToString()
                : null;

            // 5. Montar e retornar o FeedResponseDto.
            return new FeedResponseDto
            {
                Posts = postDtos,
                NextCursor = nextCursor
            };
        }
    }
}
