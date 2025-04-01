using Brewserve.Core.DTOs;

namespace Brewserve.Core.Interfaces
{
    public  interface IBarService
    {
        Task<IEnumerable<BarDTO>> GetBarsAsync();
        Task<BarDTO> GetBarByIdAsync(int id);
        Task<BarDTO> AddBarAsync(CreateBarDTO barDto);
        Task UpdateBarAsync(BarDTO barDto);
    }
}
