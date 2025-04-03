using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;

namespace Brewserve.Data.Interfaces
{
    public interface IBarRepository: IRepository<Bar>
    {
        //Task<Bar> GetBarWithBeersByIdAsync(int barId);
        //Task<IEnumerable<Bar>> GetBarsWithBeersAsync();
    }
}
