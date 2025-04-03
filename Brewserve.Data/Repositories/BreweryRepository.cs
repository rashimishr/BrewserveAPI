using Brewserve.Data.Interfaces;
using BrewServe.Data.Models;
using BrewServe.Data.Repositories;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;

namespace Brewserve.Data.Repositories;

public class BreweryRepository : Repository<Brewery>, IBreweryRepository
{
    public BreweryRepository(BrewServeDbContext context) : base(context)
    {
    }

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
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}