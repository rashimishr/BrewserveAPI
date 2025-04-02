using AutoMapper;
using Brewserve.Core.Payloads;
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

        public async Task<IEnumerable<BreweryResponse>> GetBreweriesAsync()
        {
            var breweries = await _unitOfWork.Breweries.GetAllAsync();
            return _mapper.Map<IEnumerable<BreweryResponse>>(breweries);
        }

        public async Task<BreweryResponse> GetBreweryByIdAsync(int id)
        {
            var brewery = await _unitOfWork.Breweries.GetByIdAsync(id);
            return _mapper.Map<BreweryResponse>(brewery);
        }

        public async Task<BreweryResponse> AddBreweryAsync(CreateBreweryRequest brewery)
        {
            var breweryEntity = _mapper.Map<Brewery>(brewery);
            var exists = _unitOfWork.Breweries.GetAllAsync().Result.Any(b => b.Name == brewery.Name);
            if (exists)
            {
                return null;
            }
            await _unitOfWork.Breweries.AddAsync(breweryEntity);
            var savedBrewery = await _unitOfWork.SaveAsync();

            return _mapper.Map<BreweryResponse>(breweryEntity);
        }

        public async Task UpdateBreweryAsync(BreweryRequest brewery)
        {
            var breweryUpdated = await _unitOfWork.Breweries.GetByIdAsync(brewery.Id);
           
            var breweryEntity = _mapper.Map(brewery, brewery);
            _unitOfWork.Breweries.UpdateAsync(breweryUpdated);
            var updatedBrewery = await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BreweryResponse>> GetBreweryBeerLinkAsync()
        {
            var brewery = await _unitOfWork.BreweryBeersLinks.GetAssociatedBreweryBeersAsync();
            return _mapper.Map<IEnumerable<BreweryResponse>>(brewery);
        }

        public async Task<BreweryResponse> GetBreweryBeerLinkByBreweryIdAsync(int id)
        {
            var brewery = await _unitOfWork.BreweryBeersLinks.GetAssociatedBreweryBeersByBreweryIdAsync(id);
            return _mapper.Map<BreweryResponse>(brewery);
        }
        public async Task<BreweryResponse> AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeer)
        {
            var existingBrewery = _unitOfWork.Breweries.GetByIdAsync(breweryBeer.BreweryId);
            var existingBeer = _unitOfWork.Beers.GetByIdAsync(breweryBeer.BeerId);
            var existingLink = _unitOfWork.BreweryBeersLinks.GetAssociatedBreweryBeersByBreweryIdAsync(breweryBeer.BreweryId);
            if (existingLink != null || existingBrewery == null || existingBeer == null)
            {
                return null;
            }
            var breweryBeerEntity = _mapper.Map<BreweryBeerLink>(breweryBeer);
            await _unitOfWork.BreweryBeersLinks.AddAsync(breweryBeerEntity);
            var savedBar = await _unitOfWork.SaveAsync();

            return _mapper.Map<BreweryResponse>(breweryBeerEntity);
        }
    }
}