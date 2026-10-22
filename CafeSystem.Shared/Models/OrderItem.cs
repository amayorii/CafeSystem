namespace CafeSystem.Shared.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice => (Product?.Price ?? 0m) * Quantity;
    public string Notes { get; set; } = string.Empty;
}
