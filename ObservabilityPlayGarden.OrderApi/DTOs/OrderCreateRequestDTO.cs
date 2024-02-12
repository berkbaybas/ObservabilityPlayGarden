﻿namespace ObservabilityPlayGarden.OrderApi.DTOs
{
    public record OrderCreateRequestDTO
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = null!;
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
