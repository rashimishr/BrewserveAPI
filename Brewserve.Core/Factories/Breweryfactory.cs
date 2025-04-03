using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;

namespace BrewServe.Core.Factories;

public class BreweryFactory : IBreweryFactory
{
    public BreweryRequest CreateBreweryRequest(string name)
    {
        return new BreweryRequest
        {
            Name = name
        };
    }
}