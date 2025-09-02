using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> Create(Comment comment);
        Task<Comment?> GetById(long commentId);
        Task<IEnumerable<Comment>> GetByPostId(long postId, int pageNumber = 1, int pageSize = 20);
        Task<IEnumerable<Comment>> GetReplies(long commentId, int pageNumber = 1, int pageSize = 20);
        Task Update(Comment comment);
        Task Delete(long commentId);
        Task<int> GetCommentCount(long postId);
        Task<IEnumerable<Comment>> GetAll(long? postId, long? userId, EntityStatus? status, int pageNumber = 1, int pageSize = 20);
    }
}