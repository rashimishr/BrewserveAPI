using AutoMapper;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;

namespace BrewServe.Core.Services;

public class BarService : IBarService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

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

    public async Task<BarResponse> AddBarAsync(BarRequest bar)
    {
        var barEntity = _mapper.Map<Bar>(bar);
        var exists = _unitOfWork.Bars.GetAllAsync().Result.Any(b => b.Name == bar.Name && b.Address == bar.Address);
        if (exists) return null;
        await _unitOfWork.Bars.AddAsync(barEntity);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BarResponse>(barEntity);
    }

    public async Task<BarResponse> UpdateBarAsync(BarRequest bar)
    {
        var barEntity = _mapper.Map<Bar>(bar);
        barEntity.Id = bar.Id;
        await _unitOfWork.Bars.UpdateAsync(barEntity);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BarResponse>(barEntity);
    }

    public async Task<IEnumerable<BarBeerLinkResponse>> GetBarBeerLinkAsync()
    {
        var bars = await _unitOfWork.BarBeersLinks.GetAssociatedBarBeersAsync();
        return _mapper.Map<IEnumerable<BarBeerLinkResponse>>(bars);
    }

    public async Task<BarBeerLinkResponse> GetBarBeerLinkByBarIdAsync(int id)
    {
        var bar = await _unitOfWork.BarBeersLinks.GetAssociatedBarBeersByBarIdAsync(id);
        return _mapper.Map<BarBeerLinkResponse>(bar);
    }

    public async Task<BarResponse> AddBarBeerLinkAsync(BarBeerLinkRequest barBeer)
    {
        var existingBar = await _unitOfWork.Bars.GetByIdAsync(barBeer.BarId);
        var existingBeer = await _unitOfWork.Beers.GetByIdAsync(barBeer.BeerId);
        if (existingBar == null || existingBeer == null) return null;
        var barBeerEntity = _mapper.Map<BarBeerLink>(barBeer);
        await _unitOfWork.BarBeersLinks.AddAsync(barBeerEntity);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BarResponse>(barBeerEntity);
    }
}