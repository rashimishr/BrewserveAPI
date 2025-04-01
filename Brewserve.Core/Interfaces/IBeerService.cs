using Brewserve.Core.DTOs;
using Brewserve.Data.Models;

namespace Brewserve.Core.Interfaces
{
    public interface IBeerService
    {
        Task<IEnumerable<BeerDTO>> GetBeersAsync();
        Task<BeerDTO> GetBeerByIdAsync(int id);
        Task<BeerDTO> AddBeerAsync(CreateBeerDTO beerDto);
        Task UpdateBeerAsync(BeerDTO beerDto);
        Task<IEnumerable<BeerDTO>> GetBeersByAlcoholContentAsync(decimal gtAlcoholByVolume, decimal ltAlcoholByVolume);
    }
}
