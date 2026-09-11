using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class OrderService : BaseService<Order, Guid>
{
    public OrderService(CafeDbContext db) : base(db)
    {
    }
}
