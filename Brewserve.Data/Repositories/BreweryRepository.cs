using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories
{
    public class BreweryRepository : Repository<Brewery>, IBreweryRepository
    {
        public BreweryRepository(BrewserveDbContext context) : base(context){}

        public async Task<IEnumerable<Brewery>> GetBreweriesWithBeersAsync()
        {
            return await _context.Breweries
                .Include(b => b.BreweryBeers)
                .ThenInclude(bb => bb.Beer)
                .ToListAsync();
        }
        public async Task<Brewery> GetBreweryWithBeersByIdAsync(int id)
        {
            return await _context.Breweries
                .Include(b => b.BreweryBeers)
                .ThenInclude(bb => bb.Beer)
                .FirstOrDefaultAsync(b =>b.Id == id);
        }
    }
}
