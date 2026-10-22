using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class OrdersController : BaseController<Order, int>
{
    public OrdersController(OrderService service) : base(service)
    {
    }
}
