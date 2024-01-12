﻿

using ObservabilityPlayGarden.ConsoleApp;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using var traceProvider = Sdk.CreateTracerProviderBuilder()
                        .AddSource(StringConsts.ActivitySourceName)
                        .ConfigureResource(resource =>
                                resource.AddService(
                                    serviceName: StringConsts.ServiceName,
                                    serviceVersion: StringConsts.Version)
                                    .AddAttributes(new List<KeyValuePair<string, object>>(){
                                        new KeyValuePair<string, object>("host.machineName", Environment.MachineName)
                                    }))
                         .AddConsoleExporter()
                         .AddOtlpExporter()
                         .Build();


//ServiceHelper.Work1();
await ServiceHelper.Work2();