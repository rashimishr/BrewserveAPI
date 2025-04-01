using Brewserve.Core.DTOs;

namespace Brewserve.Core.Interfaces
{
    public interface IBeerFactory
    {
        BeerDTO CreateBeerDTO(string name, decimal percentageAlcoholByVolume);
    }
}
