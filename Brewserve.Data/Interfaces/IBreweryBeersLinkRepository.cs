using Brewserve.Data.Models;

namespace Brewserve.Data.Interfaces
{
    public interface IBreweryBeersLinkRepository: IRepository<BreweryBeerLink>
    {
        Task<IEnumerable<Brewery>> GetAssociatedBreweryBeersByBreweryIdAsync(int breweryId);
        Task<IEnumerable<Brewery>> GetAssociatedBreweryBeersAsync();
    }
}
