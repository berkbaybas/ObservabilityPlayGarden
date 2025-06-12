using System.Diagnostics.Metrics;

namespace ObservabilityPlayGarden.OpenTelemetry.Shared;

public static class MetricProvider
{
    public static readonly Meter meter = new("metric.meter.api");
    public static Counter<int> OrderCreatedEventCounter = meter.CreateCounter<int>("order.created.event.counter");
    public static Counter<int> OrderHistoryRequestCounter = meter.CreateCounter<int>("order.history.event.counter");
    public static Counter<int> OrderCancelledCounter = meter.CreateCounter<int>("order.cancelled.event.counter");
}
