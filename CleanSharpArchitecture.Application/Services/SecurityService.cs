using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;
using CleanSharpArchitecture.Domain.Interfaces;
using Serilog;

namespace CleanSharpArchitecture.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowerRepository _followerRepository;

        public SecurityService(
            IUserRepository userRepository,
            IFollowerRepository followerRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _followerRepository = followerRepository ?? throw new ArgumentNullException(nameof(followerRepository));
        }

        public async Task<bool> CanModifyPostAsync(long userId, Post post)
        {
            if (post == null)
            {
                Log.Warning("Attempt to check permissions on null post by user {UserId}", userId);
                return false;
            }

            if (!await IsValidUserAsync(userId))
            {
                Log.Warning("Invalid user {UserId} attempting to modify post {PostId}", userId, post.Id);
                return false;
            }

            var isOwner = post.UserId == userId;
            
            if (!isOwner)
            {
                Log.Warning("User {UserId} attempted to modify post {PostId} owned by {PostOwnerId}", 
                    userId, post.Id, post.UserId);
            }

            return isOwner;
        }

        public bool CanModifyUser(long currentUserId, long targetUserId)
        {
            var canModify = currentUserId == targetUserId;
            
            if (!canModify)
            {
                Log.Warning("User {CurrentUserId} attempted to modify user {TargetUserId}", 
                    currentUserId, targetUserId);
            }

            return canModify;
        }

        public async Task<bool> CanViewProfileAsync(long viewerId, long profileUserId)
        {
            if (viewerId == profileUserId)
                return true;

            if (!await IsValidUserAsync(viewerId) || !await IsValidUserAsync(profileUserId))
            {
                Log.Warning("Invalid user in profile view attempt. Viewer: {ViewerId}, Profile: {ProfileUserId}", 
                    viewerId, profileUserId);
                return false;
            }

            // All profiles are public for now
            return true;
        }

        public bool CanFollowUser(long followerId, long followedId)
        {
            var canFollow = followerId != followedId;
            
            if (!canFollow)
            {
                Log.Warning("User {UserId} attempted to follow themselves", followerId);
            }

            return canFollow;
        }

        public async Task<bool> CanLikePostAsync(long userId, Post post)
        {
            if (post == null)
            {
                Log.Warning("Attempt to check like permissions on null post by user {UserId}", userId);
                return false;
            }

            if (!await IsValidUserAsync(userId))
            {
                Log.Warning("Invalid user {UserId} attempting to like post {PostId}", userId, post.Id);
                return false;
            }

            return true;
        }

        public async Task<bool> CanCommentOnPostAsync(long userId, Post post)
        {
            if (post == null)
            {
                Log.Warning("Attempt to check comment permissions on null post by user {UserId}", userId);
                return false;
            }

            if (!await IsValidUserAsync(userId))
            {
                Log.Warning("Invalid user {UserId} attempting to comment on post {PostId}", userId, post.Id);
                return false;
            }

            return true;
        }

        public async Task<bool> IsValidUserAsync(long userId)
        {
            if (userId <= 0)
                return false;

            try
            {
                var user = await _userRepository.SelectById(userId);
                return user != null && user.Status == Domain.Enums.EntityStatus.Active;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking if user {UserId} is valid", userId);
                return false;
            }
        }

        //public async Task<bool> IsFollowingAsync(long followerId, long followedId)
        //{
        //    if (!await IsValidUserAsync(followerId) || !await IsValidUserAsync(followedId))
        //    {
        //        Log.Warning("Invalid user in follow check. Follower: {FollowerId}, Followed: {FollowedId}", 
        //            followerId, followedId);
        //        return false;
        //    }
        //    return await _followerRepository.IsFollowingAsync(followerId, followedId);
        //}

        public async Task<bool> CanModifyNotification(long userId, Notification notification)
        {
            if (notification == null)
            {
                Log.Warning("Attempt to check permissions on null notification by user {UserId}", userId);
                return false;
            }
            if (!await IsValidUserAsync(userId))
            {
                Log.Warning("Invalid user {UserId} attempting to modify notification {NotificationId}", userId, notification.Id);
                return false;
            }
            var isOwner = notification.UserId == userId;
            if (!isOwner)
            {
                Log.Warning("User {UserId} attempted to modify notification {NotificationId} owned by {NotificationOwnerId}",
                    userId, notification.Id, notification.UserId);
            }
            return isOwner;
        }

        public void ThrowAccessDenied(string action, string resource)
        {
            var message = $"Access denied: Cannot {action} {resource}.";
            Log.Warning("Security violation: {Message}", message);
            throw new UnauthorizedAccessException(message);
        }

        public void ThrowNotFound(string resource, long id)
        {
            var message = $"{resource} with ID {id} not found.";
            Log.Warning("Resource not found: {Message}", message);
            throw new ArgumentException(message);
        }

    }
}