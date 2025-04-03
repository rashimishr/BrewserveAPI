using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces
{
    public interface IBreweryFactory
    {
        BreweryRequest CreateBreweryRequest(string name);
    }
}
