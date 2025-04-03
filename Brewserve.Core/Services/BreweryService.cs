using AutoMapper;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
namespace BrewServe.Core.Services;
public class BreweryService : IBreweryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
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
    public async Task<BreweryResponse> AddBreweryAsync(BreweryRequest brewery)
    {
        var breweryEntity = _mapper.Map<Brewery>(brewery);
        var exists = _unitOfWork.Breweries.GetAllAsync().Result.Any(b => b.Name == brewery.Name);
        if (exists) return null;
        await _unitOfWork.Breweries.AddAsync(breweryEntity);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BreweryResponse>(breweryEntity);
    }
    public async Task<BreweryResponse> UpdateBreweryAsync(BreweryRequest brewery)
    {
        var breweryUpdated = await _unitOfWork.Breweries.GetByIdAsync(brewery.Id);
        var breweryEntity = _mapper.Map(brewery, brewery);
        breweryEntity.Id = brewery.Id;
        await _unitOfWork.Breweries.UpdateAsync(breweryUpdated);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BreweryResponse>(breweryEntity);
    }
    public async Task<IEnumerable<BreweryBeerLinkResponse>> GetBreweryBeerLinkAsync()
    {
        var brewery = await _unitOfWork.BreweryBeersLinks.GetAssociatedBreweryBeersAsync();
        return _mapper.Map<IEnumerable<BreweryBeerLinkResponse>>(brewery);
    }
    public async Task<BreweryBeerLinkResponse> GetBreweryBeerLinkByBreweryIdAsync(int id)
    {
        var brewery = await _unitOfWork.BreweryBeersLinks.GetAssociatedBreweryBeersByBreweryIdAsync(id);
        return _mapper.Map<BreweryBeerLinkResponse>(brewery);
    }
    public async Task<BreweryResponse> AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeer)
    {
        var existingBrewery = await _unitOfWork.Breweries.GetByIdAsync(breweryBeer.BreweryId);
        var existingBeer = await _unitOfWork.Beers.GetByIdAsync(breweryBeer.BeerId);
        if (existingBrewery == null || existingBeer == null) return null;
        var breweryBeerEntity = _mapper.Map<BreweryBeerLink>(breweryBeer);
        await _unitOfWork.BreweryBeersLinks.AddAsync(breweryBeerEntity);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BreweryResponse>(breweryBeerEntity);
    }
}