using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Exceptions;
using Watchdog.Bot.Models;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly DbContext Context;

    protected Repository(DbContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity?> GetByIdAsync(params object[] identity)
    {
        var entity = await Context.Set<TEntity>().FindAsync(identity);
        if (entity == null) return null;
        
        Context.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public virtual async Task<int> GetCountAsync()
    {
        return await Context.Set<TEntity>().CountAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {        
        var createdEntity = await Context.Set<TEntity>().AddAsync(entity);

        try
        {
            await Context.SaveChangesAsync();
            return createdEntity.Entity;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await EntityExists(entity.GetIdentity()))
                throw new ObjectExistsException("Entity already exists");
            throw;            
        }
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Modified;

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EntityExists(entity.GetIdentity()))
                throw new ObjectNotFoundException("Entity not found");
            throw;
        }
    }

    public virtual async Task DeleteAsync(params object[] identity)
    {
        var entity = await GetByIdAsync(identity);
        if (entity == null)
            throw new ObjectNotFoundException("Entity not found");

        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
    }

    protected async Task<bool> EntityExists(params object[] identity)
    {
        return await GetByIdAsync(identity) != null;
    }
}