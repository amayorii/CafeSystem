using System.ComponentModel.DataAnnotations.Schema;

namespace CafeSystem.Shared.Models;

public class Shift
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime EndTIme { get; set; } = DateTime.UtcNow;
    public decimal TotalRevenue { get; set; }
    public List<Order> Orders { get; set; } = [];

    [NotMapped]
    public IEnumerable<ShiftProductSummary> ProductBreakdown => Orders.SelectMany(order => order.Items)
                                                                        .Where(item => item.Product != null)
                                                                        .GroupBy(item => item.Product.Name)
                                                                        .Select(group => new ShiftProductSummary
                                                                        {
                                                                            ProductName = group.Key,
                                                                            QuantitySold = group.Sum(i => i.Quantity),
                                                                            TotalRevenue = group.Sum(i => i.TotalPrice)
                                                                        })
                                                                        .OrderByDescending(x => x.TotalRevenue);
    public decimal TotalTips { get; set; }

    public decimal CalculatedSalary => Employee.BaseSalary + TotalRevenue * (Employee.RevenuePercentage / 100m);
}