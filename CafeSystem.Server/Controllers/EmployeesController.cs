using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class EmployeesController : BaseController<Employee, int>
{
    public EmployeesController(EmployeeService service) : base(service)
    {
    }
}
