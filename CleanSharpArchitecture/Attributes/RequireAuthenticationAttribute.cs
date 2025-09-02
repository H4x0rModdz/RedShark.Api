using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CleanSharpArchitecture.API.Attributes
{
    /// <summary>
    /// Attribute that ensures the user is authenticated and adds the userId to the context
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireAuthenticationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            // Check if the user is authenticated
            if (!user.Identity?.IsAuthenticated == true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Extract the userId from the token
            var currentUserId = GetCurrentUserId(user);
            if (currentUserId == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid user token");
                return;
            }

            // Add the userId to the context for use in the controller
            context.HttpContext.Items["CurrentUserId"] = currentUserId.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private long? GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? user.FindFirst("sub")?.Value
                           ?? user.FindFirst("userId")?.Value
                           ?? user.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            return long.TryParse(userIdClaim, out var userId) && userId > 0 ? userId : null;
        }
    }
}