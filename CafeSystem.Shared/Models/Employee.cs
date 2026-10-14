using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Shared.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public EmployeeRole Role { get; set; }
    [MaxLength(10)]
    public string PinCode { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; } = 800.00m;
    public decimal RevenuePercentage { get; set; } = 13.00m;
}

public enum EmployeeRole
{
    Barista,
    Intern
}
