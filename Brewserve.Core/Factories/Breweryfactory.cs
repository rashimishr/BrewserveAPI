using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class Breweryfactory : IBreweryFactory
    {
        public BreweryRequest CreateBreweryRequest(string name)
        {
            return new BreweryRequest
            {
                Name = name,
            };
        }
    }
}
