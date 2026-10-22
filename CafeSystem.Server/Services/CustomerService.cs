using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class CustomerService : BaseService<Customer, int>
{
    public CustomerService(CafeDbContext db) : base(db)
    {
    }
}
