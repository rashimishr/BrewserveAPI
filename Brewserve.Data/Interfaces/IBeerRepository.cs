using Brewserve.Data.Models;

namespace Brewserve.Data.Interfaces
{
    public interface IBeerRepository: IRepository<Beer>
    {
        Task<IEnumerable<Beer>> GetBeersByAlcoholContentAsync(decimal gtAlcoholContent, decimal ltAlcoholContent);
    }
}
