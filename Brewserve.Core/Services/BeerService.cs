using AutoMapper;
using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Strategies;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;

namespace Brewserve.Core.Services
{
    public class BeerService: IBeerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BeerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BeerDTO>> GetBeersAsync()
        {
            var beers = await _unitOfWork.Beers.GetAllAsync();
            return _mapper.Map<IEnumerable<BeerDTO>>(beers);
        }

        public async Task<BeerDTO> GetBeerByIdAsync(int id)
        {
            var beer = await _unitOfWork.Beers.GetByIdAsync(id);
            return _mapper.Map<BeerDTO>(beer);
        }

        public async Task<BeerDTO> AddBeerAsync(CreateBeerDTO beerDto)
        {
            var beerEntity = _mapper.Map<Beer>(beerDto);

            await _unitOfWork.Beers.AddAsync(beerEntity);
            var beer = await _unitOfWork.SaveAsync();

            return _mapper.Map<BeerDTO>(beer);
        }

        public async Task UpdateBeerAsync(BeerDTO beerDto)
        {
            var beer = _mapper.Map<Beer>(beerDto);
            _unitOfWork.Beers.UpdateAsync(beer);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BeerDTO>> GetBeersByAlcoholContentAsync(decimal gtAlcoholByVolume, decimal ltAlcoholByVolume)
        {
           var strategy = new BeerByAlcoholContentStrategy(_unitOfWork, _mapper);

           return await strategy.FilterAsync();
        }
    }
}
