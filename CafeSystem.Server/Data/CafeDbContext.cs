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
}
