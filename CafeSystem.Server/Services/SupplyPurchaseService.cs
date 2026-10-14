using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class SupplyPurchaseService : BaseService<SupplyPurchase, int>
{
    public SupplyPurchaseService(CafeDbContext db) : base(db)
    {

    }
}