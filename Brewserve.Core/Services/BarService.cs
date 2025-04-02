using AutoMapper;
using Brewserve.Core.Payloads;
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

        public async Task<IEnumerable<BarResponse>> GetBarsAsync()
        {
            var bars = await _unitOfWork.Bars.GetAllAsync();
            return _mapper.Map<IEnumerable<BarResponse>>(bars);
        }

        public async Task<BarResponse> GetBarByIdAsync(int id)
        {
            var bar = await _unitOfWork.Bars.GetByIdAsync(id);
            return _mapper.Map<BarResponse>(bar);
        }

        public async Task<BarResponse> AddBarAsync(CreateBarRequest bar)
        {
            var barEntity = _mapper.Map<Bar>(bar);

            var exists = _unitOfWork.Bars.GetAllAsync().Result.Any(b => b.Name == bar.Name && b.Address==bar.Address);
            if (exists)
            {
                return null;
            }
            await _unitOfWork.Bars.AddAsync(barEntity);
            var savedBar = await _unitOfWork.SaveAsync();

            return _mapper.Map<BarResponse>(barEntity);
        }
        public async Task UpdateBarAsync(BarRequest bar)
        {
            var barEntity = _mapper.Map<Bar>(bar);
            _unitOfWork.Bars.UpdateAsync(barEntity);
            var updatedBar = await _unitOfWork.SaveAsync();
        }
    }
}
