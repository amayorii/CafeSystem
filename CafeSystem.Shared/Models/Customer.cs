using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Shared.Models;

public class Customer
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LoyaltyPoints { get; set; }
}