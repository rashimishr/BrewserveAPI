using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;

namespace Brewserve.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BrewserveDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(BrewserveDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public async Task UpdateAsync(TEntity entity) =>  _dbSet.Update(entity);
        public async Task SaveAsync(int id) => await _context.SaveChangesAsync();

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
