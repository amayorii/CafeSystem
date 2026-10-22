using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Services;

public class ShiftService : BaseService<Shift, int>
{
    public ShiftService(CafeDbContext db) : base(db)
    {
    }

    public override async Task<IEnumerable<Shift>> GetAllAsync()
    {
        return await _db.Shifts
            .Include(s => s.Employee)
            .ToListAsync();
    }

    public override async Task<Shift?> GetByIdAsync(int id)
    {
        return await _db.Shifts
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<ShiftProductSummary>> GetProductSummaryAsync(int shiftId)
    {
        var shift = await _db.Shifts.Include(s => s.Orders)
                                    .ThenInclude(o => o.Items)
                                    .ThenInclude(i => i.Product)
                                    .FirstOrDefaultAsync(s => s.Id == shiftId);

        return shift?.ProductBreakdown ?? [];
    }
}
