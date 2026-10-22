using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class CustomersController : BaseController<Customer, int>
{
    public CustomersController(CustomerService service) : base(service)
    {
    }
}
