using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like> Create(Like like);

        Task Delete(long likeId);

        Task<Like?> GetById(long likeId);

        Task<IEnumerable<Like>> GetAll(long? postId, EntityStatus? status, int pageNumber = 1, int pageSize = 10);

        Task<Like?> GetByUserAndPost(long userId, long postId);

        Task<Like?> GetByUserAndComment(long userId, long commentId);

        Task<int> GetLikesCountByPostId(long postId);

        Task<int> GetLikesCountByCommentId(long commentId);
    }
}
