using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class CategoryService : BaseService<Category, int>
{
    public CategoryService(CafeDbContext db) : base(db)
    {
    }
}
