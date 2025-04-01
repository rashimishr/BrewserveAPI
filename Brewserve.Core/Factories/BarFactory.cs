
using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;

namespace Brewserve.Core.Factories
{
    public class BarFactory: IBarFactory
    {
        public BarDTO CreateBarDTO(string name, string address)
        {
            return new BarDTO
            {
                Name = name,
                Address = address
            };
        }
    }
}
