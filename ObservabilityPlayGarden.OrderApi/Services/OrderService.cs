using ObservabilityPlayGarden.OpenTelemetry.Shared;
using ObservabilityPlayGarden.OrderApi.DTOs;
using System.Diagnostics;

namespace ObservabilityPlayGarden.OrderApi.Services
{
    public class OrderService
    {
        public Task CreateAsync(OrderCreateRequestDTO requestDTO)
        {
            Activity.Current.SetTag("Asp.Net core instrumentation tag1", "span tag1");
            using var activity = ActivitySourceProvider.Source.StartActivity();
            activity?.AddEvent(new("Sipariş süreci başladı."));
            activity.AddTag("order user id", requestDTO.UserId);
            activity?.AddEvent(new("Sipariş süreci tamamlandı."));
            return Task.CompletedTask;
        }
    }
}
