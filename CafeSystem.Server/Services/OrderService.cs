using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Services;

public class OrderService : BaseService<Order, int>
{
    public OrderService(CafeDbContext db) : base(db)
    {
    }

    public override async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _db.Orders
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
