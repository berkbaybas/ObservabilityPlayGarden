using ObservabilityPlayGarden.ConsoleApp;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

# region get-environment
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
               ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
               ?? "unknown";
#endregion

using var traceProvider = Sdk.CreateTracerProviderBuilder()
                        .AddSource(StringConsts.ActivitySourceName)
                        .ConfigureResource(resource =>
                                resource.AddService(
                                    serviceName: StringConsts.ServiceName,
                                    serviceVersion: StringConsts.Version)
                                    .AddAttributes(new List<KeyValuePair<string, object>>(){
                                        new KeyValuePair<string, object>("host.machineName", Environment.MachineName),
                                        new KeyValuePair<string, object>("host.os", Environment.OSVersion.VersionString),
                                        new KeyValuePair<string, object>("host.environment", environment),
                                        new KeyValuePair<string, object>("dotnet.version", Environment.Version.ToString())
                                    }))
                         .AddConsoleExporter()
                         .AddOtlpExporter()
                         .Build();

using var fileTraceProvider = Sdk.CreateTracerProviderBuilder()
                                .AddSource(StringConsts.ActivitySourceFileName)
                                .Build();


//ServiceHelper.Work1();
await ServiceHelper.Work2();

