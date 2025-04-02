using Brewserve.Data.Models;

namespace Brewserve.Data.Interfaces
{
    public interface IBarBeersLinkRepository: IRepository<BarBeerLink>
    {
        Task<IEnumerable<Bar>> GetAssociatedBarBeersByBarIdAsync(int barId);
        Task<IEnumerable<Bar>> GetAssociatedBarBeersAsync();
    }
}
