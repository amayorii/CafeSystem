namespace CafeSystem.Shared.Models;

public class Transaction
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal ChangeGiven { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum PaymentMethod
{
    Cash,
    Card
}
