using System.Diagnostics.Metrics;

namespace ObservabilityPlayGarden.OpenTelemetry.Shared;

public static class MetricProvider
{
    public static readonly Meter meter = new("metric.meter.api");
    public static Counter<int> OrderCreatedEventCounter = meter.CreateCounter<int>("order.created.event.counter");
}
