using System.Text.Json;
using DocumentProcessor.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DocumentProcessor.Api.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches unhandled exceptions
    /// and returns RFC 7807 compliant ProblemDetails responses
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception occurred. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
                    context.Request.Path,
                    context.Request.Method,
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            // Determine status code based on exception type
            var (statusCode, title) = exception switch
            {
                DomainException domainEx => (domainEx.StatusCode, GetTitleForStatusCode(domainEx.StatusCode)),
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Bad Request"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            context.Response.StatusCode = statusCode;

            // Create RFC 7807 compliant ProblemDetails
            var problemDetails = new ProblemDetails
            {
                Type = GetTypeForStatusCode(statusCode),
                Title = title,
                Status = statusCode,
                Detail = GetDetailMessage(exception, statusCode),
                Instance = context.Request.Path
            };

            // Add trace ID for debugging
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            // In development, include exception details
            if (_environment.IsDevelopment())
            {
                problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }

        private string GetDetailMessage(Exception exception, int statusCode)
        {
            // For domain exceptions, use the exception message
            if (exception is DomainException)
            {
                return exception.Message;
            }

            // In production, don't expose internal error details
            if (!_environment.IsDevelopment())
            {
                return statusCode switch
                {
                    400 => "The request was invalid. Please check your input and try again.",
                    403 => "You do not have permission to access this resource.",
                    _ => "An unexpected error occurred. Please try again later or contact support if the problem persists."
                };
            }

            // In development, show the actual error message
            return exception.Message;
        }

        private static string GetTitleForStatusCode(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            403 => "Forbidden",
            404 => "Not Found",
            422 => "Business Rule Violation",
            500 => "Internal Server Error",
            _ => "Error"
        };

        private static string GetTypeForStatusCode(int statusCode) => statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
            500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            _ => "https://tools.ietf.org/html/rfc7231"
        };
    }
}
