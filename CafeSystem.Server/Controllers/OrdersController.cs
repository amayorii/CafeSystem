using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Server.Controllers;

public class OrdersController : BaseController<Order, int>
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService service) : base(service)
    {
        _orderService = service;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<Order>> CheckoutAsync([FromBody] CreateOrderRequest request)
    {
        if (request.Items == null || !request.Items.Any())
        {
            return BadRequest("Cannot checkout an empty order.");
        }

        try
        {
            var order = await _orderService.CheckoutAsync(request);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating order: {ex.Message}");
        }
    }
}
