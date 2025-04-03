using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces;

public interface IBarService
{
    Task<IEnumerable<BarResponse>> GetBarsAsync();
    Task<BarResponse> GetBarByIdAsync(int id);
    Task<BarResponse> AddBarAsync(BarRequest bar);
    Task<BarResponse> UpdateBarAsync(BarRequest bar);
    Task<IEnumerable<BarBeerLinkResponse>> GetBarBeerLinkAsync();
    Task<BarBeerLinkResponse> GetBarBeerLinkByBarIdAsync(int id);
    Task<BarResponse> AddBarBeerLinkAsync(BarBeerLinkRequest barBeer);
}