using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ObservabilityPlayGarden.OpenTelemetry.Shared;
using ObservabilityPlayGarden.OrderApi.DTOs;

namespace ObservabilityPlayGarden.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create(OrderCreateRequestDTO dto)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity("Order.Create")!;

            activity.SetTag("user.id", dto.UserId);
            activity.SetTag("order.totalPrice", dto.TotalPrice);
            activity.AddEvent(new("Order creation started"));

            try
            {
                if (dto.TotalPrice <= 0)
                    throw new ArgumentException("Total price must be greater than zero.");

                MetricProvider.OrderCreatedEventCounter.Add(1,
                    new KeyValuePair<string, object?>("queue-name", "event.created.queue"));

                _logger.LogInformation("Order created for user {UserId} with total {TotalPrice}",
                    dto.UserId, dto.TotalPrice);

                activity?.AddEvent(new("Order creation completed"));
                return Ok(new { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order for user {UserId}", dto.UserId);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                return StatusCode(500, "Unexpected error");
            }
        }

        [HttpGet("list")]
        public IActionResult GetAllOrderHistory()
        {
            _logger.LogInformation("Fetching orders history");

            var orders = new List<object>
            {
                new { OrderId = 1, Price = 150, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new { OrderId = 2, Price = 200, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new { OrderId = 3, Price = 1200, CreatedAt = DateTime.UtcNow.AddDays(-3) }
            };
            
            MetricProvider.OrderHistoryRequestCounter.Add(1);

            return Ok(orders);
        }

        [HttpDelete("{orderId}")]
        public IActionResult CancelOrder(long orderId)
        {            
            // Trace
            using var activity = ActivitySourceProvider.Source.StartActivity("Order.Cancel");

            activity?.SetTag("order.id", orderId);
            _logger.LogWarning("Order cancellation requested: {OrderId}", orderId);
            activity?.AddEvent(new("Cancellation initiated"));

            // Metric
            MetricProvider.OrderCancelledCounter.Add(1, new KeyValuePair<string, object?>("order-id", orderId.ToString()));
            _logger.LogInformation("Order cancelled: {OrderId}", orderId);

            return Ok(new { Message = "Order cancelled" });
        }
    }
}
