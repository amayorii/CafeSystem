namespace CafeSystem.Shared.Models;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;

    public decimal TotalPrice => Product.Price * Quantity;
}
