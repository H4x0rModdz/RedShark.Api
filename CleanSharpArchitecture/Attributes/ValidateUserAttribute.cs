using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CleanSharpArchitecture.API.Attributes
{
    /// <summary>
    /// Attribute that validates if the specified user exists and is active
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateUserAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _userIdParameter;
        private readonly bool _requireAuthentication;

        public ValidateUserAttribute(string userIdParameter = "userId", bool requireAuthentication = false)
        {
            _userIdParameter = userIdParameter;
            _requireAuthentication = requireAuthentication;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            
            // If authentication is required, check if authenticated
            if (_requireAuthentication)
            {
                var user = httpContext.User;
                if (!user.Identity?.IsAuthenticated == true)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Add the userId from token to the context
                var currentUserId = GetCurrentUserId(user);
                if (currentUserId.HasValue)
                {
                    context.HttpContext.Items["CurrentUserId"] = currentUserId.Value;
                }
            }

            // Extract the userId from the parameter
            var targetUserId = GetTargetUserId(context);
            if (targetUserId == null)
            {
                context.Result = new BadRequestObjectResult($"Parameter '{_userIdParameter}' not found or invalid");
                return;
            }

            // Validate if the user exists
            var securityService = httpContext.RequestServices.GetRequiredService<ISecurityService>();
            
            if (!await securityService.IsValidUserAsync(targetUserId.Value))
            {
                context.Result = new NotFoundObjectResult($"User with ID {targetUserId.Value} not found or inactive");
                return;
            }

            await next();
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

        private long? GetTargetUserId(ActionExecutingContext context)
        {
            // Try to get from route parameters first
            if (context.RouteData.Values.TryGetValue(_userIdParameter, out var routeValue))
            {
                if (long.TryParse(routeValue?.ToString(), out var routeId) && routeId > 0)
                    return routeId;
            }

            // Try to get from action parameters
            if (context.ActionArguments.TryGetValue(_userIdParameter, out var paramValue))
            {
                if (long.TryParse(paramValue?.ToString(), out var paramId) && paramId > 0)
                    return paramId;
            }

            return null;
        }
    }
}