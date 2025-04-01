
using Brewserve.Core.DTOs;

namespace Brewserve.Core.Interfaces
{
    public interface IBarFactory
    {
        BarDTO CreateBarDTO(string name, string address);
    }
}
