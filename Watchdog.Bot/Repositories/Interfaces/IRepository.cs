using System.Linq.Expressions;
using Watchdog.Bot.Models;

namespace Watchdog.Bot.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(params object[] identity);
    Task<int> GetCountAsync();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(params object[] identity);
}