using CafeSystem.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Services;

public class BaseService<TEntity, TId> : IBaseService<TEntity, TId> where TEntity : class
{
    protected readonly CafeDbContext _db;

    public BaseService(CafeDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _db.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        // FindAsync is perfect here because it takes a generic object for the key, 
        // regardless of whether it is an int or a Guid.
        return await _db.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        _db.Set<TEntity>().Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _db.Set<TEntity>().Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
