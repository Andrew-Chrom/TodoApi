using TodoApi.Errors;

namespace TodoApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (UnathorizedException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                _logger.LogError(ex, "An unexpected error occurred");
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
            }
        }

    }
}
