namespace ObservabilityPlayGarden.OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = null!; // => runtime'da bir etkisi yok. EF core nullable column oluşturmaz ve compiler null olabilir hatası verdirmez.
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public enum OrderStatus : byte
    {
        Fail = 0,
        Success = 1
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
