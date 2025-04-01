using AutoMapper;
using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;

namespace Brewserve.Core.Services
{
    public class BarService: IBarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BarDTO>> GetBarsAsync()
        {
            var bars = await _unitOfWork.Bars.GetAllAsync();
            return _mapper.Map<IEnumerable<BarDTO>>(bars);
        }

        public async Task<BarDTO> GetBarByIdAsync(int id)
        {
            var bar = await _unitOfWork.Bars.GetByIdAsync(id);
            return _mapper.Map<BarDTO>(bar);
        }

        public async Task<BarDTO> AddBarAsync(CreateBarDTO barDto)
        {
            var barEntity = _mapper.Map<Bar>(barDto);

            await _unitOfWork.Bars.AddAsync(barEntity);
            var bar = await _unitOfWork.SaveAsync();

            return _mapper.Map<BarDTO>(bar);
        }
        public async Task UpdateBarAsync(BarDTO barDto)
        {
            var bar = _mapper.Map<Bar>(barDto);
            _unitOfWork.Bars.UpdateAsync(bar);
            await _unitOfWork.SaveAsync();
        }
    }
}
