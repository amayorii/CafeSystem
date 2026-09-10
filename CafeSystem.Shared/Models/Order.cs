namespace CafeSystem.Shared.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int TableNumber { get; set; }
    public List<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum OrderStatus
{
    Open,
    Closed
}
