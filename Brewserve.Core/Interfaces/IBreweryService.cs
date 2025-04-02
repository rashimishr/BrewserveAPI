using Brewserve.Core.Payloads;

namespace Brewserve.Core.Interfaces
{
    public interface IBreweryService
    {
       Task<IEnumerable<BreweryResponse>> GetBreweriesAsync();
       Task<BreweryResponse> GetBreweryByIdAsync(int id);
       Task<BreweryResponse> AddBreweryAsync(CreateBreweryRequest brewery);
       Task UpdateBreweryAsync(BreweryRequest brewery);
       Task<IEnumerable<BreweryResponse>> GetBreweryBeerLinkAsync();
       Task<BreweryResponse> GetBreweryBeerLinkByBreweryIdAsync(int id);
        Task<BreweryResponse> AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeer);
    }
}
