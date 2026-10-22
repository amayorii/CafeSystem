using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class EmployeeService : BaseService<Employee, int>
{
    public EmployeeService(CafeDbContext db) : base(db)
    {
    }
}
