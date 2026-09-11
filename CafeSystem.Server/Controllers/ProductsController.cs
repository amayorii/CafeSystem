using CafeSystem.Server.Data;
using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Server.Controllers;

public class ProductsController : BaseController<Product, int>
{
    private readonly CafeDbContext _db;

    public ProductsController(ProductService service, CafeDbContext db) : base(service)
    {
        _db = db;
    }

    [HttpPost("seed")]
    public async Task<ActionResult> ForceSeed()
    {
        try
        {
            await DbSeeder.SeedAsync(_db);
            return Ok("Seeding completed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Seeding failed: {ex.Message} \n {ex.InnerException?.Message}");
        }
    }
}