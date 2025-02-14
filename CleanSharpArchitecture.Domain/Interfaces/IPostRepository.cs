using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post> Create(Post post);
        Task<Post?> GetById(long id);
        Task<IEnumerable<Post>> GetAll(int pageNumber = 1, int pageSize = 10);
        Task Update(Post post);
        Task Delete(long id);
        Task<IEnumerable<PostImage>> GetImagesByPostId(long postId);
        Task AddImages(IEnumerable<PostImage> images);
        Task RemoveImages(IEnumerable<PostImage> images);
        Task<IEnumerable<Post>> GetPostsForFeed(IEnumerable<long> followedUserIds, string? cursor, int pageSize);
    }
}
