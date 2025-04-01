using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories
{
    public class BarRepository: Repository<Bar>, IBarRepository
    {
        public BarRepository(BrewserveDbContext context) : base(context) { }

        public async Task<IEnumerable<Bar>> GetBarsWithBeersAsync()
        {
            return await _context.Bars
                .Include(b => b.BarBeers)
                .ThenInclude(bb => bb.Beer)
                .ToListAsync();
        }

        public async Task<Bar> GetBarWithBeersByIdAsync(int id)
        {
            return await _context.Bars
                .Include(b => b.BarBeers)
                .ThenInclude(bb => bb.Beer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
