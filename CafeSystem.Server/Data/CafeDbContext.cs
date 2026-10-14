using Microsoft.EntityFrameworkCore;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Data;

public class CafeDbContext : DbContext
{
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<SupplyPurchase> SupplyPurchases { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ProductModifier> ProductModifiers { get; set; }
}
