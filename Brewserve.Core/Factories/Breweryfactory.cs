using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class Breweryfactory : IBreweryFactory
    {
        public BreweryDTO CreateBreweryDTO(string name)
        {
            return new BreweryDTO
            {
                Name = name,
            };
        }
    }
}
