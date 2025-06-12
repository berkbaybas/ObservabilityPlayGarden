namespace ObservabilityPlayGarden.OrderApi.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // bir sonraki middleware'e geç
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception occurred. Stack Trace: {ex.StackTrace}");

            var response = new ErrorResponse
            {
                Title = "An unexpected error occurred.",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Status;

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    public class ErrorResponse
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
    }
}