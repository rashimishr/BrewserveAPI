

using Brewserve.Core.DTOs;

namespace Brewserve.Core.Interfaces
{
    public interface IBreweryService
    {
       Task<IEnumerable<BreweryDTO>> GetBreweriesAsync();
       Task<BreweryDTO> GetBreweryByIdAsync(int id);
       Task<BreweryDTO> AddBreweryAsync(CreateBreweryDTO breweryDto);
       Task UpdateBreweryAsync(int id, BreweryDTO breweryDto);
    }
}
