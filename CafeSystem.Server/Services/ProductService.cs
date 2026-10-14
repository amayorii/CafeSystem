using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Services;

public class ProductService : BaseService<Product, int>
{
    public ProductService(CafeDbContext db) : base(db)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _db.Products
            .Include(p => p.Category)
            .ToListAsync();
    }
}
