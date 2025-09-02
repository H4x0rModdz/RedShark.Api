using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> CreateComment(long postId, long userId, string content, long? parentCommentId = null);
        Task<CommentDto?> GetCommentById(long commentId);
        Task<IEnumerable<CommentDto>> GetCommentsByPost(long postId, int pageNumber = 1, int pageSize = 20);
        Task<IEnumerable<CommentDto>> GetCommentReplies(long commentId, int pageNumber = 1, int pageSize = 20);
        Task<CommentDto?> UpdateComment(long commentId, long userId, string newContent);
        Task<bool> DeleteComment(long commentId, long userId);
        Task<IEnumerable<CommentDto>> GetAllComments(long? postId, long? userId, EntityStatus? status, int pageNumber = 1, int pageSize = 20);
    }
}