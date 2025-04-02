using AutoMapper;
using Brewserve.Core.Payloads;
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

        public async Task<IEnumerable<BeerResponse>> GetBeersAsync()
        {
            var beers = await _unitOfWork.Beers.GetAllAsync();
            return _mapper.Map<IEnumerable<BeerResponse>>(beers);
        }

        public async Task<BeerResponse> GetBeerByIdAsync(int id)
        {
            var beer = await _unitOfWork.Beers.GetByIdAsync(id);
            return _mapper.Map<BeerResponse>(beer);
        }

        public async Task<BeerResponse> AddBeerAsync(CreateBeerRequest beer)
        {
            var beerEntity = _mapper.Map<Data.Models.Beer>(beer);

            var exists = _unitOfWork.Beers.GetAllAsync().Result.Any(b => b.Name == beer.Name);
            if(exists)
            {
                return null;
            }
            await _unitOfWork.Beers.AddAsync(beerEntity);
            var savedBeer = await _unitOfWork.SaveAsync();

            return _mapper.Map<BeerResponse>(beerEntity);
        }

        public async Task UpdateBeerAsync(BeerRequest beer)
        {
            var beerEntity = _mapper.Map<Data.Models.Beer>(beer);
            await _unitOfWork.Beers.UpdateAsync(beerEntity);
            var updatedBeer = await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BeerResponse>> GetBeersByAlcoholContentAsync(decimal gtAlcoholByVolume, decimal ltAlcoholByVolume)
        {
           var strategy = new BeerByAlcoholContentStrategy(_unitOfWork, _mapper);

           return await strategy.FilterAsync();
        }
    }
}
