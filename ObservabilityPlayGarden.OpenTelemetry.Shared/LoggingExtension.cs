using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ObservabilityPlayGarden.OpenTelemetry.Shared
{
    public class LoggingExtension
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;
                loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithProcessId()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                .WriteTo.Console();

                if (env.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Campaign", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Damage", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Identity", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("InsurancePolicy", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Integration", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Notification", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Reaction", LogEventLevel.Debug);
                }

                //Elastic Search
                var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
                var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");

                if (!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUrl))
                        {
                            AutoRegisterTemplate = true, // create automatic index
                            ModifyConnectionSettings = conn => conn.BasicAuthentication(username, password),
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                            IndexFormat = $"observability-logs-{env.ApplicationName}-{env.EnvironmentName.ToLower()}-" + "{0:yyyy.MM.dd}",
                            MinimumLogEventLevel = env.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information,
                            OverwriteTemplate = true,
                            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                            CustomFormatter = new ElasticsearchJsonFormatter(),

                        });
                }

                SelfLog.Enable(Console.Out); // Hatayi console yaz.
            };
    }
}
