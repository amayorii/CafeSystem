namespace CafeSystem.Shared.Models;

public class CreateOrderRequest
{
    public int? EmployeeId { get; set; }
    public int? ShiftId { get; set; }
    public int? CustomerId { get; set; }
    public int TableNumber { get; set; } = 1;
    public decimal TipAmount { get; set; } = 0;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;
    public decimal CashTendered { get; set; } = 0;
    public decimal ChangeGiven { get; set; } = 0;
    public List<CreateOrderItemDto> Items { get; set; } = [];
}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public string Notes { get; set; } = string.Empty;
}
