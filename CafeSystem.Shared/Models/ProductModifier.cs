namespace CafeSystem.Shared.Models;

public class ProductModifier
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
}