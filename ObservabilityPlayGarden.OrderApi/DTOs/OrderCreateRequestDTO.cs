namespace ObservabilityPlayGarden.OrderApi.DTOs
{
    public record OrderCreateRequestDTO
    {
        public long UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = null!;
        public decimal TotalPrice => Items?.Sum(i => i.Count * i.UnitPrice) ?? 0;
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
