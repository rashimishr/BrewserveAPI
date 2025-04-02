using Brewserve.Core.Payloads;

namespace Brewserve.Core.Interfaces
{
    public interface IBreweryFactory
    {
        BreweryRequest CreateBreweryRequest(string name);
    }
}
