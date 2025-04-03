using BrewServe.Data.Models;

namespace BrewServe.Data.Interfaces
{
    public interface IBreweryBeersLinkRepository : IRepository<BreweryBeerLink>
    {
        Task<Brewery> GetAssociatedBreweryBeersByBreweryIdAsync(int breweryId);
        Task<IEnumerable<Brewery>> GetAssociatedBreweryBeersAsync();
    }
}
