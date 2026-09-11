using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class ProductService : BaseService<Product, int>
{
    public ProductService(CafeDbContext db) : base(db)
    {
    }
}
