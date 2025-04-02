
using Brewserve.Core.Payloads;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;
using Brewserve.Data.Repositories;

namespace Brewserve.Core.Interfaces
{
    public interface IBeerSearchStrategy
    {
        Task<IEnumerable<BeerResponse>> FilterAsync();
    }
}
