using BrewServe.Core.Payloads;

namespace BrewServe.Core.Interfaces
{
    public interface IBarFactory
    {
        BarRequest CreateBarRequest(string name, string address);
    }
}
