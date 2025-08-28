using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;

namespace CleanSharpArchitecture.Application.UseCases
{
    public interface ICreatePostUseCase
    {
        Task<PostResultDto> ExecuteAsync(CreatePostDto request, CancellationToken cancellationToken = default);
    }

    public class CreatePostUseCase : ICreatePostUseCase
    {
        private readonly IPostService _postService;

        public CreatePostUseCase(IPostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<PostResultDto> ExecuteAsync(CreatePostDto request, CancellationToken cancellationToken = default)
        {
            // Additional business logic can be added here before calling the service
            // For example: user permissions check, rate limiting, content filtering, etc.
            
            return await _postService.CreatePost(request);
        }
    }
}