using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using CleanSharpArchitecture.Application.Exceptions;

namespace CleanSharpArchitecture.CrossCutting.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

            var response = CreateErrorResponse(exception);

            httpContext.Response.StatusCode = (int)response.StatusCode;
            httpContext.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(response.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            return true;
        }

        private ErrorResponse CreateErrorResponse(Exception exception)
        {
            return exception switch
            {
                UserNotFoundException ex => new ErrorResponse(
                    HttpStatusCode.NotFound,
                    new { message = ex.Message, type = "UserNotFound" }
                ),
                PostNotFoundException ex => new ErrorResponse(
                    HttpStatusCode.NotFound,
                    new { message = ex.Message, type = "PostNotFound" }
                ),
                BusinessRuleException ex => new ErrorResponse(
                    HttpStatusCode.BadRequest,
                    new { message = ex.Message, type = "BusinessRuleViolation" }
                ),
                ValidationException ex => new ErrorResponse(
                    HttpStatusCode.BadRequest,
                    new 
                    { 
                        message = "Validation failed", 
                        type = "ValidationError",
                        errors = ex.ValidationResults.Select(vr => new
                        {
                            field = string.Join(", ", vr.MemberNames),
                            message = vr.ErrorMessage
                        })
                    }
                ),
                ArgumentException ex => new ErrorResponse(
                    HttpStatusCode.BadRequest,
                    new { message = ex.Message, type = "InvalidArgument" }
                ),
                InvalidOperationException ex => new ErrorResponse(
                    HttpStatusCode.BadRequest,
                    new { message = ex.Message, type = "InvalidOperation" }
                ),
                UnauthorizedAccessException ex => new ErrorResponse(
                    HttpStatusCode.Unauthorized,
                    new { message = "Unauthorized access", type = "Unauthorized" }
                ),
                TimeoutException ex => new ErrorResponse(
                    HttpStatusCode.RequestTimeout,
                    new { message = "Request timeout", type = "Timeout" }
                ),
                _ => new ErrorResponse(
                    HttpStatusCode.InternalServerError,
                    new { message = "An unexpected error occurred", type = "InternalServerError" }
                )
            };
        }

        private record ErrorResponse(HttpStatusCode StatusCode, object Body);
    }
}