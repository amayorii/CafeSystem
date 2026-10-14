namespace CafeSystem.Shared.Models;

public class SupplyPurchase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}