using Brewserve.Core.Payloads;

namespace Brewserve.Core.Interfaces
{
    public interface IBeerFactory
    {
        BeerRequest CreateBeerRequest(string name, decimal percentageAlcoholByVolume);
    }
}
    