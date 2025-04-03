using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;

namespace BrewServe.Data.Repositories
{
    public class BreweryBeersLinkRepository : Repository<BreweryBeerLink>, IBreweryBeersLinkRepository
    {
        public BreweryBeersLinkRepository(BrewServeDbContext context) : base(context) { }
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
                .Include(b => b.Beer)
                .Where(bbl => bbl.BreweryId == breweryId)
                .Select(bbl => bbl.Brewery)
                .ToListAsync();
        }
    }
}
