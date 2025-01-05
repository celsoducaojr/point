using Microsoft.AspNetCore.Mvc;
using Point.Core.Application.Exceptions;
using System.Text.Json;

namespace Point.API.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Log request here...
                await next(context);
                // Log response here...
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            ProblemDetails problemDetails = new()
            {
                Status = statusCode,
                Title = GetTitle(exception),
                Detail = exception.Message
            };

            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(GetCustomProblemDetails(problemDetails, exception)));
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                DomainException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidatorException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
        private static string GetTitle(Exception exception) =>
            exception switch
            {
                DomainException => "Bad Request",
                NotFoundException => "Not Found",
                ValidatorException => "Field Validation Error",
                _ => "Server Error"
            };

        private static object GetCustomProblemDetails(ProblemDetails problemDetails, Exception exception)
        {
            if (exception is ValidatorException validationException)
            {
                problemDetails.Extensions = new Dictionary<string, object?>
                {
                    ["InvalidParams"] = validationException.Errors
                };
            }

            return problemDetails;
        }
    }
}
