using System.IO.Compression;
using AutoMapper;
using Brewserve.Data.Models;
using Brewserve.Core.DTOs;

namespace Brewserve.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateBarDTO, Bar>();
            CreateMap<Bar, BarDTO>()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BarBeers.Select(bb => new BeerDTO
                {
                    Id=bb.Beer.Id,
                    Name = bb.Beer.Name,
                    PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
                }).ToList()));

            CreateMap<CreateBeerDTO, Beer>();
            CreateMap<Beer, BeerDTO>()
                .ForMember(dest => dest.Breweries, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BreweryDTO()
                {
                    Id = bb.Brewery.Id,
                    Name = bb.Brewery.Name,
                }).ToList()));
            
            CreateMap<CreateBreweryDTO, Brewery>();
            CreateMap<Brewery, BreweryDTO>()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BeerDTO
                {
                    Id = bb.Beer.Id,
                    Name = bb.Beer.Name,
                    PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
                }).ToList()))
                .ReverseMap();
        }
    }
}
