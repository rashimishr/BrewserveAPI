using System.IO.Compression;
using AutoMapper;
using Brewserve.Data.Models;
using Brewserve.Core.Payloads;

namespace Brewserve.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Bar Mapping
            CreateMap<CreateBarRequest, Bar>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarBeers, opt=>opt.Ignore()); // Ignore if it's not part of CreateBarRequest
            CreateMap<Bar, BarRequest>()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BarBeers.Select(bb => new BeerRequest
                {
                    Id = bb.Beer.Id,
                    Name = bb.Beer.Name,
                    PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
                }).ToList()));
            CreateMap<Bar, BarResponse>().ReverseMap();

            // Beer Mappings
            CreateMap<CreateBeerRequest, Beer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarBeers, opt => opt.Ignore())
                .ForMember(dest => dest.BreweryBeers, opt => opt.Ignore()); // Ignore if it's not part of CreateBeerRequest
            CreateMap<Beer, BeerRequest>()
                .ForMember(dest => dest.Breweries, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BreweryRequest
                {
                    Id = bb.Brewery.Id,
                    Name = bb.Brewery.Name,
                }).ToList()));
            CreateMap<Beer, BeerResponse>().ReverseMap();

            // Brewery Mappings
            CreateMap<CreateBreweryRequest, Brewery>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BreweryBeers, opt => opt.Ignore()); //Ignore if it's not part 
            CreateMap<Brewery, BreweryRequest>()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BeerRequest
                {
                    Id = bb.Beer.Id,
                    Name = bb.Beer.Name,
                    PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
                }).ToList()))
                .ReverseMap();
            CreateMap<Brewery, BreweryResponse>().ReverseMap();
        }
    }
}
