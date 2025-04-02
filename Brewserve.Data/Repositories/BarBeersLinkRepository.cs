using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories
{
    public class BarBeersLinkRepository : Repository<BarBeerLink>, IBarBeersLinkRepository
    {
        public BarBeersLinkRepository(BrewserveDbContext context) : base(context) { }

        public async Task<IEnumerable<Bar>> GetAssociatedBarBeersAsync()
        {
            return await _context.Bars
                .Include(b => b.BarBeers)
                .ThenInclude(bb => bb.Beer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bar>> GetAssociatedBarBeersByBarIdAsync(int barId)
        {
            return await _context.BarBeers
                .Where(bbl => bbl.BarId == barId)
                .Select(bbl => bbl.Bar)
                .ToListAsync();
        }
    }
}
