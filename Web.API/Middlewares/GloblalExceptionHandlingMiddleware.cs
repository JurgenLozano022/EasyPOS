using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.API.Middlewares
{
    public class GloblalExceptionHandlingMiddleware(ILogger<GloblalExceptionHandlingMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<GloblalExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ProblemDetails problemDetails = new() 
                {
                    Title = "Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An internal server has occurred.",
                    Type = "Server Error"
                };

                string json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}
