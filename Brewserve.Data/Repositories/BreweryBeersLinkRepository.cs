using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories
{
    public class BreweryBeersLinkRepository : Repository<BreweryBeerLink>, IBreweryBeersLinkRepository
    {
        public BreweryBeersLinkRepository(BrewserveDbContext context) : base(context) { }
        public async Task<IEnumerable<Brewery>> GetAssociatedBreweryBeersAsync()
        {
            return await _context.Breweries
                .Include(b => b.BreweryBeers)
                .ThenInclude(bb => bb.Beer)
                .ToListAsync();
        }
        public async Task<IEnumerable<Brewery>> GetAssociatedBreweryBeersByBreweryIdAsync(int breweryId)
        {
            return await _context.BreweryBeers
                .Where(bbl => bbl.BreweryId == breweryId)
                .Select(bbl => bbl.Brewery)
                .ToListAsync();
        }
    }
}
