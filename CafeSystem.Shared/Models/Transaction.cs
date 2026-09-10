namespace CafeSystem.Shared.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal ChangeGiven { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum PaymentMethod
{
    Cash,
    Card
}
