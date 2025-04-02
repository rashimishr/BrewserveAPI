using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class BeerFactory: IBeerFactory
    {
        public BeerRequest CreateBeerRequest(string name, decimal percentageAlcoholByVolume)
        {
            return new BeerRequest
            {
                Name = name,
                PercentageAlcoholByVolume = percentageAlcoholByVolume
            };
        }
    }
}
