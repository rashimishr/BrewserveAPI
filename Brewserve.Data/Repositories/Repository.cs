using System.Linq.Expressions;
using BrewServe.Data.Interfaces;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;
namespace BrewServe.Data.Repositories;
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly BrewServeDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    public Repository(BrewServeDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    public async Task<TEntity> GetByIdAsync(int? id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    public async Task<IEnumerable<TEntity>> GetAllAssociatedLinkedAsync(
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var includeProperty in includeProperties) query = query.Include(includeProperty);
        return await query.ToListAsync();
    }
}