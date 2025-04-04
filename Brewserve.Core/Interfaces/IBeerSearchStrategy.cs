using BrewServe.Core.Payloads;
namespace BrewServe.Core.Interfaces;
public interface IBeerSearchStrategy
{
    Task<IEnumerable<BeerResponse>> FilterAsync();
    void SetAlcoholContent(decimal? gtAlcoholByVolume, decimal? ltAlcoholByVolume);
}