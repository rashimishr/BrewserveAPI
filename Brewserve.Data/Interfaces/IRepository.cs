using System.Linq.Expressions;

namespace BrewServe.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int? id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAssociatedLinkedAsync(
        params Expression<Func<TEntity, object>>[] includeProperties);
}