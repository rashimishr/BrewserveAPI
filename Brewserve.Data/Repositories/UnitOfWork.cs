using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;

namespace Brewserve.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly BrewserveDbContext _context;
        public IBarRepository Bars { get; }
        public IBeerRepository Beers { get; private set; }
        public IBreweryRepository Breweries { get; private set; }
        public IBreweryBeersLinkRepository BreweryBeersLinks { get; private set; }
        public IBarBeersLinkRepository BarBeersLinks { get; private set; }

        public UnitOfWork(BrewserveDbContext context)
        {
            _context = context;

            Bars = new BarRepository(context);
            Beers = new BeerRepository(context);
            Breweries = new BreweryRepository(context);
            BreweryBeersLinks = new BreweryBeersLinkRepository(context);
            BarBeersLinks = new BarBeersLinkRepository(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

       public void Dispose() => _context.Dispose();
    }
}
