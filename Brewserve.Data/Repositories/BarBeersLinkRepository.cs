using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;

namespace BrewServe.Data.Repositories
{
    public class BarBeersLinkRepository : Repository<BarBeerLink>, IBarBeersLinkRepository
    {
        public BarBeersLinkRepository(BrewServeDbContext context) : base(context) { }
        public async Task<IEnumerable<Bar>> GetAssociatedBarBeersAsync()
        {
            return await _context.Bars
                .Include(b => b.BarBeers)
                .ThenInclude(bb => bb.Beer)
                .ToListAsync();
        }
        public async Task<Bar> GetAssociatedBarBeersByBarIdAsync(int barId)
        {
            return await _context.Bars
                .Include(b=>b.BarBeers)
                .ThenInclude(bb=>bb.Beer)
                .FirstOrDefaultAsync(bbl => bbl.Id == barId);
       }
    }
}
