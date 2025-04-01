using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;

namespace Brewserve.Core.Strategies
{
    public class BeerSearchContext
    {
        private IBeerSearchStrategy _strategy;

        public void SetStrategy(IBeerSearchStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<IEnumerable<BeerDTO>> FilterAsync()
        {
            if(_strategy == null)
            {
                throw new InvalidOperationException("Strategy not set");
            }
            return await _strategy.FilterAsync();
        }
    }
}
