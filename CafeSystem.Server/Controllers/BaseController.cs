using CafeSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntity, TId> : ControllerBase where TEntity : class
{
    protected readonly IBaseService<TEntity, TId> _service;

    protected BaseController(IBaseService<TEntity, TId> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntity>> GetByIdAsync(TId id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
            return NotFound();
            
        return Ok(item);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TEntity>> CreateAsync([FromBody] TEntity entity)
    {
        var createdItem = await _service.CreateAsync(entity);
        return Ok(createdItem);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult> UpdateAsync(TId id, [FromBody] TEntity entity)
    {
        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> DeleteAsync(TId id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        await _service.DeleteAsync(id);
        return NoContent();
    }
}
