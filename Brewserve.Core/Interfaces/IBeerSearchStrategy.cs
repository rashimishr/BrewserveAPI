using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces;

public interface IBeerSearchStrategy
{
    Task<IEnumerable<BeerResponse>> FilterAsync();
}