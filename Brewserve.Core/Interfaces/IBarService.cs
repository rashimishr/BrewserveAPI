using Brewserve.Core.Payloads;

namespace Brewserve.Core.Interfaces
{
    public  interface IBarService
    {
        Task<IEnumerable<BarResponse>> GetBarsAsync();
        Task<BarResponse> GetBarByIdAsync(int id);
        Task<BarResponse> AddBarAsync(CreateBarRequest bar);
        Task UpdateBarAsync(BarRequest bar);
        Task<IEnumerable<BarResponse>> GetBarBeerLinkAsync();
        Task<BarResponse> GetBarBeerLinkByBarIdAsync(int id);
        Task<BarResponse> AddBarBeerLinkAsync(BarBeerLinkRequest barBeer);
    }
}
