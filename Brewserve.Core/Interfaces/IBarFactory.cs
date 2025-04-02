
using Brewserve.Core.Payloads;

namespace Brewserve.Core.Interfaces
{
    public interface IBarFactory
    {
        BarRequest CreateBarRequest(string name, string address);
    }
}
