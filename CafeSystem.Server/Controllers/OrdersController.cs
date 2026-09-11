using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class OrdersController : BaseController<Order, Guid>
{
    public OrdersController(OrderService service) : base(service)
    {
    }
}
