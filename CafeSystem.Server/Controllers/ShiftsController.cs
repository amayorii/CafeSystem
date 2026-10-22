using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Server.Controllers;

public class ShiftsController : BaseController<Shift, int>
{
    private readonly ShiftService _shiftService;

    public ShiftsController(ShiftService service) : base(service)
    {
        _shiftService = service;
    }

    [HttpGet("{id}/summary")]
    public async Task<ActionResult<IEnumerable<ShiftProductSummary>>> GetProductSummary(int id)
    {
        var summary = await _shiftService.GetProductSummaryAsync(id);
        return Ok(summary);
    }
}
