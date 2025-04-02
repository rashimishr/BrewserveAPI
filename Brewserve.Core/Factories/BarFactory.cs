
using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class BarFactory: IBarFactory
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
