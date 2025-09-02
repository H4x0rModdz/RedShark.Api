using CleanSharpArchitecture.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CleanSharpArchitecture.API.Attributes
{
    /// <summary>
    /// Attribute that verifies if the authenticated user is the owner of the post being accessed
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequirePostOwnershipAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _postIdParameter;

        public RequirePostOwnershipAttribute(string postIdParameter = "postId")
        {
            _postIdParameter = postIdParameter;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
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

            // Extract the postId from the parameters
            var postId = GetPostId(context);
            if (postId == null)
            {
                context.Result = new BadRequestObjectResult($"Parameter '{_postIdParameter}' not found or invalid");
                return;
            }

            // Fetch the post to verify ownership
            var postRepository = httpContext.RequestServices.GetRequiredService<IPostRepository>();
            var post = await postRepository.GetById(postId.Value);
            
            if (post == null)
            {
                context.Result = new NotFoundObjectResult($"Post with ID {postId.Value} not found");
                return;
            }

            // Check if the user is the owner of the post
            if (post.UserId != currentUserId.Value)
            {
                context.Result = new ForbidResult($"Access denied: User {currentUserId.Value} is not the owner of post {postId.Value}");
                return;
            }

            // Add information to the context for use in the controller
            context.HttpContext.Items["CurrentUserId"] = currentUserId.Value;
            context.HttpContext.Items["PostId"] = postId.Value;
            context.HttpContext.Items["Post"] = post;
            
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

        private long? GetPostId(ActionExecutingContext context)
        {
            // Try to get from route parameters first
            if (context.RouteData.Values.TryGetValue(_postIdParameter, out var routeValue))
            {
                if (long.TryParse(routeValue?.ToString(), out var routeId) && routeId > 0)
                    return routeId;
            }

            // Try to get from action parameters
            if (context.ActionArguments.TryGetValue(_postIdParameter, out var paramValue))
            {
                if (long.TryParse(paramValue?.ToString(), out var paramId) && paramId > 0)
                    return paramId;
            }

            // For DTOs, try to extract the PostId from the object
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null) continue;
                
                var postIdProperty = arg.GetType().GetProperty("PostId") ?? 
                                   arg.GetType().GetProperty("Id");
                
                if (postIdProperty != null)
                {
                    var idValue = postIdProperty.GetValue(arg);
                    if (long.TryParse(idValue?.ToString(), out var dtoId) && dtoId > 0)
                        return dtoId;
                }
            }

            return null;
        }
    }
}