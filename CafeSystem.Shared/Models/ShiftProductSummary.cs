namespace CafeSystem.Shared.Models;

public class ShiftProductSummary
{
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}