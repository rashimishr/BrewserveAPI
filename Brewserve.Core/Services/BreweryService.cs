using AutoMapper;
using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;

namespace Brewserve.Core.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BreweryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BreweryDTO>> GetBreweriesAsync()
        {
            var breweries = await _unitOfWork.Breweries.GetAllAsync();
            return _mapper.Map<IEnumerable<BreweryDTO>>(breweries);
        }

        public async Task<BreweryDTO> GetBreweryByIdAsync(int id)
        {
            var brewery = await _unitOfWork.Breweries.GetByIdAsync(id);
            return _mapper.Map<BreweryDTO>(brewery);
        }

        public async Task<BreweryDTO> AddBreweryAsync(CreateBreweryDTO breweryDto)
        {
            var breweryEntity = _mapper.Map<Brewery>(breweryDto);
            await _unitOfWork.Breweries.AddAsync(breweryEntity);
            var brewery = await _unitOfWork.SaveAsync();

            return _mapper.Map<BreweryDTO>(brewery);
        }

        public async Task UpdateBreweryAsync(int id, BreweryDTO breweryDto)
        {
            var brewery = await _unitOfWork.Breweries.GetByIdAsync(id);
            if (brewery == null)
            {
                throw new Exception("Brewery not found");
            }
            _mapper.Map(breweryDto, brewery);
            _unitOfWork.Breweries.UpdateAsync(brewery);
            await _unitOfWork.SaveAsync();
        }

    }
}
