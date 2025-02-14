using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostResultDto> CreatePost(CreatePostDto postDto);
        Task<PostResultDto> UpdatePost(UpdatePostDto postDto);
        Task<PostResultDto> DeletePost(long postId);
        Task<IEnumerable<GetPostDto>> GetAllPosts(int pageNumber = 1, int pageSize = 10);
        Task<GetPostDto?> GetPostById(long postId);
    }
}
