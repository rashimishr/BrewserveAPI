using Brewserve.Core.DTOs;

namespace Brewserve.Core.Interfaces
{
    public interface IBreweryFactory
    {
        BreweryDTO CreateBreweryDTO(string name);
    }
}
