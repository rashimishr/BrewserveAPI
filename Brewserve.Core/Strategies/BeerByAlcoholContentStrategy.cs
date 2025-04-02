using AutoMapper;
using AutoMapper.QueryableExtensions;
using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;
using Brewserve.Data.Interfaces;
using Brewserve.Data.Models;

namespace Brewserve.Core.Strategies
{
    public class BeerByAlcoholContentStrategy : IBeerSearchStrategy
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private decimal _gtAlcoholByVolume;
        private decimal _ltAlcoholByVolume;

        public BeerByAlcoholContentStrategy(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void SetAlcoholContent(decimal gtAlcoholByVolume, decimal ltAlcoholByVolume)
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
