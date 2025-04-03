using Brewserve.Data.Interfaces;

namespace BrewServe.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBarRepository Bars { get; }
        IBeerRepository Beers { get; }
        IBreweryRepository Breweries { get; }
        IBreweryBeersLinkRepository BreweryBeersLinks { get; }
        IBarBeersLinkRepository BarBeersLinks { get; }
        Task<int> SaveAsync();
    }
}
