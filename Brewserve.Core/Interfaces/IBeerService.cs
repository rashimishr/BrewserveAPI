using Brewserve.Core.Payloads;
using Brewserve.Data.Models;

namespace Brewserve.Core.Interfaces
{
    public interface IBeerService
    {
        Task<IEnumerable<BeerResponse>> GetBeersAsync();
        Task<BeerResponse> GetBeerByIdAsync(int id);
        Task<BeerResponse> AddBeerAsync(CreateBeerRequest beer);
        Task UpdateBeerAsync(BeerRequest beer);
        Task<IEnumerable<BeerResponse>> GetBeersByAlcoholContentAsync(decimal gtAlcoholByVolume, decimal ltAlcoholByVolume);
    }
}
