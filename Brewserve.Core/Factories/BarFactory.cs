using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;

namespace BrewServe.Core.Factories
{
    public class BarFactory : IBarFactory
    {
        public BarRequest CreateBarRequest(string name, string address)
        {
            return new BarRequest
            {
                Name = name,
                Address = address
            };
        }
    }
}
