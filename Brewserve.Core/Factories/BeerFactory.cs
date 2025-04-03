using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;

namespace BrewServe.Core.Factories;

public class BeerFactory : IBeerFactory
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