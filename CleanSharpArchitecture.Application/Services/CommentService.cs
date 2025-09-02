using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Application.Interfaces.Repositories;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using Serilog;

namespace CleanSharpArchitecture.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<CommentDto> CreateComment(long postId, long userId, string content, long? parentCommentId = null)
        {
            try
            {
                var post = await _postRepository.GetById(postId);
                if (post == null)
                {
                    throw new ArgumentException("Post not found");
                }

                if (parentCommentId.HasValue)
                {
                    var parentComment = await _commentRepository.GetById(parentCommentId.Value);
                    if (parentComment == null || parentComment.PostId != postId)
                    {
                        throw new ArgumentException("Parent comment not found or belongs to different post");
                    }
                }

                var comment = new Comment
                {
                    Content = content,
                    UserId = userId,
                    PostId = postId,
                    ParentCommentId = parentCommentId,
                    Status = EntityStatus.Active
                };

                var createdComment = await _commentRepository.Create(comment);
                return _mapper.Map<CommentDto>(createdComment);
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating comment: {ex.Message}");
                throw;
            }
        }

        public async Task<CommentDto?> GetCommentById(long commentId)
        {
            try
            {
                var comment = await _commentRepository.GetById(commentId);
                return comment == null ? null : _mapper.Map<CommentDto>(comment);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting comment by id: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPost(long postId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var comments = await _commentRepository.GetByPostId(postId, pageNumber, pageSize);
                return _mapper.Map<IEnumerable<CommentDto>>(comments);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting comments by post: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CommentDto>> GetCommentReplies(long commentId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var replies = await _commentRepository.GetReplies(commentId, pageNumber, pageSize);
                return _mapper.Map<IEnumerable<CommentDto>>(replies);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting comment replies: {ex.Message}");
                throw;
            }
        }

        public async Task<CommentDto?> UpdateComment(long commentId, long userId, string newContent)
        {
            try
            {
                var comment = await _commentRepository.GetById(commentId);
                
                if (comment == null)
                {
                    return null;
                }

                if (comment.UserId != userId)
                {
                    throw new UnauthorizedAccessException("User is not the owner of this comment");
                }

                comment.Content = newContent;
                comment.UpdatedAt = DateTime.UtcNow;

                await _commentRepository.Update(comment);
                return _mapper.Map<CommentDto>(comment);
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating comment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteComment(long commentId, long userId)
        {
            try
            {
                var comment = await _commentRepository.GetById(commentId);
                
                if (comment == null)
                {
                    return false;
                }

                if (comment.UserId != userId)
                {
                    throw new UnauthorizedAccessException("User is not the owner of this comment");
                }

                await _commentRepository.Delete(commentId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting comment: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CommentDto>> GetAllComments(long? postId, long? userId, EntityStatus? status, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var comments = await _commentRepository.GetAll(postId, userId, status, pageNumber, pageSize);
                return _mapper.Map<IEnumerable<CommentDto>>(comments);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting all comments: {ex.Message}");
                throw;
            }
        }
    }
}