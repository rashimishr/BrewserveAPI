using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces;

public interface IBreweryService
{
    Task<IEnumerable<BreweryResponse>> GetBreweriesAsync();
    Task<BreweryResponse> GetBreweryByIdAsync(int id);
    Task<BreweryResponse> AddBreweryAsync(BreweryRequest brewery);
    Task<BreweryResponse> UpdateBreweryAsync(BreweryRequest brewery);
    Task<IEnumerable<BreweryBeerLinkResponse>> GetBreweryBeerLinkAsync();
    Task<BreweryBeerLinkResponse> GetBreweryBeerLinkByBreweryIdAsync(int id);
    Task<BreweryResponse> AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeer);
}