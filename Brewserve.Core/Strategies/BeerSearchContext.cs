using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
namespace BrewServe.Core.Strategies;
public class BeerSearchContext
{
    private IBeerSearchStrategy? _strategy;
    public void SetStrategy(IBeerSearchStrategy strategy)
    {
        _strategy = strategy;
    }
    public async Task<IEnumerable<BeerResponse>> FilterAsync()
    {
        if (_strategy == null) throw new InvalidOperationException("Strategy not set");
        return await _strategy.FilterAsync();
    }
}