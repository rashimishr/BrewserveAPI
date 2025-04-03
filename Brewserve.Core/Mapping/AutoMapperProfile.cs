using AutoMapper;
using BrewServe.Core.Payloads;
using BrewServe.Data.Models;
namespace BrewServe.Core.Mapping;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //Bar Mapping
        CreateMap<BarRequest, Bar>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BarBeers, opt => opt.Ignore()) // Ignore if it's not part of CreateBarRequest
            .ReverseMap();
        CreateMap<Bar, BarResponse>().ReverseMap();
        // Beer Mappings
        CreateMap<BeerRequest, Beer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BarBeers, opt => opt.Ignore())
            .ForMember(dest => dest.BreweryBeers,
                opt => opt.Ignore()) // Ignore if it's not part of CreateBeerRequest
            .ReverseMap();
        CreateMap<Beer, BeerResponse>().ReverseMap();
        // Brewery Mappings
        CreateMap<BreweryRequest, Brewery>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BreweryBeers, opt => opt.Ignore()) //Ignore if it's not part 
            .ReverseMap();
        CreateMap<Brewery, BreweryResponse>();
        // BarBeerLink Mappings
        CreateMap<BarBeerLink, BarBeerLinkRequest>();
        CreateMap<Bar, BarBeerLinkResponse>()
            .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BarBeers.Select(bbl => bbl.Beer)));
        // BreweryBeerLink Mappings
        CreateMap<BreweryBeerLink, BreweryBeerLinkRequest>();
        CreateMap<Brewery, BreweryBeerLinkResponse>()
            .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BreweryBeers.Select(bbl => bbl.Beer)));
    }
}