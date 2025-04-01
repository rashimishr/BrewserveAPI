using Brewserve.Data.EF_Core;
using Brewserve.Data.Interfaces;

namespace Brewserve.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly BrewserveDbContext _context;
        public IBarRepository Bars { get; }
        public IBeerRepository Beers { get; }
        public IBreweryRepository Breweries { get; }

        public UnitOfWork(BrewserveDbContext context)
        {
            _context = context;

            Bars = new BarRepository(context);
            Beers = new BeerRepository(context);
            Breweries = new BreweryRepository(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

       public void Dispose() => _context.Dispose();
    }
}
