using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
namespace Brewserve.Data.Interfaces;
public interface IBreweryRepository : IRepository<Brewery>
{
    Task<Brewery> GetBreweryWithBeersByIdAsync(int breweryId);
    Task<IEnumerable<Brewery>> GetBreweriesWithBeersAsync();
}