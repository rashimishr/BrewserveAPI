using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces
{
    public interface IBeerFactory
    {
        BeerRequest CreateBeerRequest(string name, decimal percentageAlcoholByVolume);
    }
}
