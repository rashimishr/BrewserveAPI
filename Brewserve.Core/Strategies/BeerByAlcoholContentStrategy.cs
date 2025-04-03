using AutoMapper;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using BrewServe.Data.Interfaces;

namespace BrewServe.Core.Strategies
{
    public class BeerByAlcoholContentStrategy : IBeerSearchStrategy
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private decimal? _gtAlcoholByVolume;
        private decimal? _ltAlcoholByVolume;
        public BeerByAlcoholContentStrategy(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void SetAlcoholContent(decimal? gtAlcoholByVolume, decimal? ltAlcoholByVolume)
        {
            _gtAlcoholByVolume = gtAlcoholByVolume;
            _ltAlcoholByVolume = ltAlcoholByVolume;
        }
        public async Task<IEnumerable<BeerResponse>> FilterAsync()
        {
            var beers = await _unitOfWork.Beers.GetBeersByAlcoholContentAsync(_gtAlcoholByVolume, _ltAlcoholByVolume);
            return _mapper.Map<IEnumerable<BeerResponse>>(beers);
        }
    }
}
