using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using BrewServeData.EF_Core;
using Microsoft.EntityFrameworkCore;

namespace BrewServe.Data.Repositories;

public class BeerRepository : Repository<Beer>, IBeerRepository
{
    public BeerRepository(BrewServeDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Beer>> GetBeersByAlcoholContentAsync(decimal? gtAlcoholContent,
        decimal? ltAlcoholContent)
    {
        IQueryable<Beer> query = _context.Beers;
        if (gtAlcoholContent.HasValue) query = query.Where(b => b.PercentageAlcoholByVolume >= gtAlcoholContent);
        if (ltAlcoholContent.HasValue) query = query.Where(b => b.PercentageAlcoholByVolume <= ltAlcoholContent);
        return await query.ToListAsync();
    }
}