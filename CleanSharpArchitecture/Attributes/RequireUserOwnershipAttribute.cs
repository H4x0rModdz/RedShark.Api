using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CleanSharpArchitecture.API.Attributes
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireUserOwnershipAttribute : Attribute, IActionFilter
    {
        private readonly string _userIdParameter;

        public RequireUserOwnershipAttribute(string userIdParameter = "id")
        {
            _userIdParameter = userIdParameter;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (!user.Identity?.IsAuthenticated == true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Extrai o userId do token
            var currentUserId = GetCurrentUserId(user);
            if (currentUserId == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid user token");
                return;
            }

            var targetUserId = GetTargetUserId(context);
            if (targetUserId == null)
            {
                context.Result = new BadRequestObjectResult($"Parameter '{_userIdParameter}' not found or invalid");
                return;
            }

            if (currentUserId.Value != targetUserId.Value)
            {
                context.Result = new ForbidResult($"Access denied: User {currentUserId.Value} cannot modify user {targetUserId.Value}");
                return;
            }

            context.HttpContext.Items["CurrentUserId"] = currentUserId.Value;
            context.HttpContext.Items["TargetUserId"] = targetUserId.Value;
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

        private long? GetTargetUserId(ActionExecutingContext context)
        {
            // Tenta pegar do route parameters primeiro
            if (context.RouteData.Values.TryGetValue(_userIdParameter, out var routeValue))
            {
                if (long.TryParse(routeValue?.ToString(), out var routeId) && routeId > 0)
                    return routeId;
            }

            // Tenta pegar dos action parameters
            if (context.ActionArguments.TryGetValue(_userIdParameter, out var paramValue))
            {
                if (long.TryParse(paramValue?.ToString(), out var paramId) && paramId > 0)
                    return paramId;
            }

            // Para DTOs, tenta extrair o ID do objeto
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null) continue;
                
                var idProperty = arg.GetType().GetProperty("Id") ?? 
                               arg.GetType().GetProperty("UserId");
                
                if (idProperty != null)
                {
                    var idValue = idProperty.GetValue(arg);
                    if (long.TryParse(idValue?.ToString(), out var dtoId) && dtoId > 0)
                        return dtoId;
                }
            }

            return null;
        }
    }
}