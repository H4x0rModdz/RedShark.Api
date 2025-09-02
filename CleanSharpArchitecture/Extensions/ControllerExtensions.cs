using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSharpArchitecture.API.Extensions
{
    public static class ControllerExtensions
    {
        public static long GetCurrentUserId(this ControllerBase controller)
        {
            if (controller.HttpContext.Items.TryGetValue("CurrentUserId", out var userIdObj) && userIdObj is long userId)
            {
                return userId;
            }

            // Fallback to manual token extraction
            return ExtractUserIdFromToken(controller);
        }

        private static long ExtractUserIdFromToken(ControllerBase controller)
        {
            if (controller.User?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? controller.User.FindFirst("sub")?.Value
                           ?? controller.User.FindFirst("userId")?.Value
                           ?? controller.User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            if (!long.TryParse(userIdClaim, out var userId) || userId <= 0)
            {
                throw new UnauthorizedAccessException("Invalid user ID in token.");
            }

            return userId;
        }

        public static long? TryGetCurrentUserId(this ControllerBase controller)
        {
            try
            {
                return controller.GetCurrentUserId();
            }
            catch
            {
                return null;
            }
        }

        public static bool IsCurrentUser(this ControllerBase controller, long targetUserId)
        {
            var currentUserId = controller.TryGetCurrentUserId();
            return currentUserId.HasValue && currentUserId.Value == targetUserId;
        }

        public static void ValidateUserAccess(this ControllerBase controller, long targetUserId, string action)
        {
            var currentUserId = controller.GetCurrentUserId();
            if (currentUserId != targetUserId)
            {
                throw new UnauthorizedAccessException($"Access denied: Cannot {action} for user {targetUserId}. Current user: {currentUserId}");
            }
        }
    }
}