using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories
{
    public class BeerRepository : Repository<Beer>, IBeerRepository
    {
        public BeerRepository(BrewserveDbContext context) : base(context) { }
        public async Task<IEnumerable<Beer>> GetBeersByAlcoholContentAsync(decimal gtAlcoholContent, decimal ltAlcoholContent)
        {
            return await _context.Beers.
                Where(b => b.PercentageAlcoholByVolume >= gtAlcoholContent && b.PercentageAlcoholByVolume <= ltAlcoholContent)
                .ToListAsync();
        }
    }
}
