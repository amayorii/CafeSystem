using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class ProductModifierService : BaseService<ProductModifier, int>
{
    public ProductModifierService(CafeDbContext db) : base(db)
    {
    }
}
