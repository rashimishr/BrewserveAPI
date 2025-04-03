using BrewServe.Data.Models;

namespace BrewServe.Data.Interfaces
{
    public interface IBarBeersLinkRepository : IRepository<BarBeerLink>
    {
        Task<IEnumerable<Bar>> GetAssociatedBarBeersByBarIdAsync(int barId);
        Task<IEnumerable<Bar>> GetAssociatedBarBeersAsync();
    }
}
