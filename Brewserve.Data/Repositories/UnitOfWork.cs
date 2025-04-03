using Brewserve.Data.Interfaces;
using BrewServe.Data.Interfaces;
using Brewserve.Data.Repositories;
using BrewServeData.EF_Core;

namespace BrewServe.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BrewServeDbContext _context;

    public UnitOfWork(BrewServeDbContext context)
    {
        _context = context;
        Bars = new BarRepository(context);
        Beers = new BeerRepository(context);
        Breweries = new BreweryRepository(context);
        BreweryBeersLinks = new BreweryBeersLinkRepository(context);
        BarBeersLinks = new BarBeersLinkRepository(context);
    }

    public IBarRepository Bars { get; }
    public IBeerRepository Beers { get; }
    public IBreweryRepository Breweries { get; }
    public IBreweryBeersLinkRepository BreweryBeersLinks { get; }
    public IBarBeersLinkRepository BarBeersLinks { get; }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}