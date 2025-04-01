using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class BeerFactory: IBeerFactory
    {
        public BeerDTO CreateBeerDTO(string name, decimal percentageAlcoholByVolume)
        {
            return new BeerDTO
            {
                Name = name,
                PercentageAlcoholByVolume = percentageAlcoholByVolume
            };
        }
    }
}
