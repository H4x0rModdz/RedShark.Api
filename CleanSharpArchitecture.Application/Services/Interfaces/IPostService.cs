using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostResultDto> CreatePost(CreatePostDto postDto);
        Task<PostResultDto> UpdatePost(UpdatePostDto postDto, long currentUserId);
        Task<PostResultDto> DeletePost(long postId, long currentUserId);
        Task<IEnumerable<PostDto>> GetAllPosts(int pageNumber = 1, int pageSize = 10);
        Task<PostDto?> GetPostById(long postId);
        Task<IEnumerable<PostDto>> GetPostsByUserId(long userId, int pageNumber = 1, int pageSize = 10);
        Task<int> GetPostsCountByUserId(long userId);
    }
}
