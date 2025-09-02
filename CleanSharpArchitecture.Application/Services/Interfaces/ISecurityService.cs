using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ISecurityService
    {
        /// <summary>
        /// Checks if the user has permission to edit/delete a post
        /// </summary>
        /// <param name="userId">ID of the user making the request</param>
        /// <param name="post">Post being modified</param>
        /// <returns>True if authorized</returns>
        Task<bool> CanModifyPostAsync(long userId, Post post);

        /// <summary>
        /// Checks if the user has permission to edit/delete a user
        /// </summary>
        /// <param name="currentUserId">ID of the user making the request</param>
        /// <param name="targetUserId">ID of the user being modified</param>
        /// <returns>True if authorized</returns>
        bool CanModifyUser(long currentUserId, long targetUserId);

        /// <summary>
        /// Checks if the user has permission to view a private profile
        /// </summary>
        /// <param name="viewerId">ID of the user viewing</param>
        /// <param name="profileUserId">ID of the profile user</param>
        /// <returns>True if authorized</returns>
        Task<bool> CanViewProfileAsync(long viewerId, long profileUserId);

        /// <summary>
        /// Checks if the user has permission to follow/unfollow another user
        /// </summary>
        /// <param name="followerId">ID of the user who wants to follow</param>
        /// <param name="followedId">ID of the user to be followed</param>
        /// <returns>True if authorized</returns>
        bool CanFollowUser(long followerId, long followedId);

        /// <summary>
        /// Checks if the user has permission to like a post
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="post">Post being liked</param>
        /// <returns>True if authorized</returns>
        Task<bool> CanLikePostAsync(long userId, Post post);

        /// <summary>
        /// Checks if the user has permission to comment on a post
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="post">Post being commented on</param>
        /// <returns>True if authorized</returns>
        Task<bool> CanCommentOnPostAsync(long userId, Post post);

        /// <summary>
        /// Checks if the user exists and is active
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>True if user is valid</returns>
        Task<bool> IsValidUserAsync(long userId);

        /// <summary>
        /// Throws access denied exception with custom message
        /// </summary>
        /// <param name="action">Action being performed</param>
        /// <param name="resource">Resource being accessed</param>
        void ThrowAccessDenied(string action, string resource);

        /// <summary>
        /// Throws resource not found exception
        /// </summary>
        /// <param name="resource">Resource not found</param>
        /// <param name="id">ID of the resource</param>
        void ThrowNotFound(string resource, long id);
    }
}