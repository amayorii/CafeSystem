namespace CafeSystem.Shared.Models;

public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public int? CustomerId { get; set; } = null;
    public Customer? Customer { get; set; } = null;
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
    public int TableNumber { get; set; }

    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public decimal TipAmount { get; set; }

    public List<OrderItem> Items { get; set; } = [];
    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum OrderStatus
{
    Open,
    Closed
}
