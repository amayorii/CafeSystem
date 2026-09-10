using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly CafeDbContext _db;
    public ProductsController(CafeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
    {
        return Ok(await _db.Products.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product newProduct)
    {
        _db.Products.Add(newProduct);

        await _db.SaveChangesAsync();
        return Ok(newProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProductAsync(int id, [FromBody] Product updatedProduct)
    {
        if (id != updatedProduct.Id)
        {
            return BadRequest();
        }

        var product = await _db.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.Category = updatedProduct.Category;
        product.ImageUrl = updatedProduct.ImageUrl;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveProductAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is not null)
            _db.Products.Remove(product);

        await _db.SaveChangesAsync();
        return NoContent();
    }
}