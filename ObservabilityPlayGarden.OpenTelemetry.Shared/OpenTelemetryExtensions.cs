using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ObservabilityPlayGarden.OpenTelemetry.Shared;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetryExt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryConstants>(configuration.GetSection("OpenTelemetry"));
        var openTelemetryConstants = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>();
        if (openTelemetryConstants is null)
        {
            throw new InvalidOperationException("OpenTelemetry configuration is missing or invalid.");
        }

        ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(openTelemetryConstants.ActivitySourceName);
        
        services.AddOpenTelemetry().WithTracing(configure =>
        {
            configure.AddSource(openTelemetryConstants.ActivitySourceName)
            .ConfigureResource(resource =>
            {
                resource.AddService(openTelemetryConstants.ServiceName, serviceVersion: openTelemetryConstants.ServiceVersion);
            });
            configure.AddAspNetCoreInstrumentation(aspnetcoreOptions =>
            {
                aspnetcoreOptions.Filter = context =>
                {
                    var path = context.Request.Path.Value;
                    return !string.IsNullOrEmpty(path) &&
                           path.Contains("api", StringComparison.OrdinalIgnoreCase);
                };

                // Serilog üzerinden elasticsearch db'ye hatalar gönderildiği için kapatıldı.
                //aspnetcoreOptions.RecordException = true;
            });

            configure.AddConsoleExporter();
            configure.AddOtlpExporter(); // Jaeger
        });

        services.AddOpenTelemetry().WithMetrics(configure =>
        {
            configure.AddMeter("metric.meter.api");
            configure.ConfigureResource(resource =>
            {
                resource.AddService(openTelemetryConstants.ServiceName, serviceVersion: openTelemetryConstants.ServiceVersion);
            });

            configure.AddConsoleExporter();
            configure.AddPrometheusExporter();
        });
    }
}
