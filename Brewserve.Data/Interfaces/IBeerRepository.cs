using BrewServe.Data.Models;

namespace BrewServe.Data.Interfaces
{
    public interface IBeerRepository : IRepository<Beer>
    {
        Task<IEnumerable<Beer>> GetBeersByAlcoholContentAsync(decimal? gtAlcoholContent, decimal? ltAlcoholContent);
    }
}
