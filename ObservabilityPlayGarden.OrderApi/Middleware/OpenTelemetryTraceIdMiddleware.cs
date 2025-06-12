using System.Diagnostics;

namespace ObservabilityPlayGarden.OrderApi.Middleware;

public class OpenTelemetryTraceIdMiddleware
{
    private readonly RequestDelegate _next;

    public OpenTelemetryTraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<OpenTelemetryTraceIdMiddleware>>();

        var traceId = Activity.Current?.TraceId.ToString();

        using (logger.BeginScope("{@traceId}", traceId))
        {
            await _next(context);
        }
    }
}