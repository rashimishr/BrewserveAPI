using AutoMapper;
using BrewServe.Core.Payloads;
using BrewServe.Data.Models;

namespace BrewServe.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Bar Mapping
            CreateMap<BarRequest, Bar>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarBeers, opt => opt.Ignore()) // Ignore if it's not part of CreateBarRequest
                .ReverseMap();
            //CreateMap<Bar, BarRequest>()
            //    .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BarBeers.Select(bb => new BeerRequest
            //    {
            //        Id = bb.Beer.Id,
            //        Name = bb.Beer.Name,
            //        PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
            //    }).ToList()));
            CreateMap<Bar, BarResponse>().ReverseMap();

            // Beer Mappings
            CreateMap<BeerRequest, Beer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarBeers, opt => opt.Ignore())
                .ForMember(dest => dest.BreweryBeers,
                    opt => opt.Ignore()) // Ignore if it's not part of CreateBeerRequest
                .ReverseMap();
            //CreateMap<Beer, BeerRequest>()
            //    .ForMember(dest => dest.Breweries, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BreweryRequest
            //    {
            //        Id = bb.Brewery.Id,
            //        Name = bb.Brewery.Name,
            //    }).ToList()));
            CreateMap<Beer, BeerResponse>().ReverseMap();

            // Brewery Mappings
            CreateMap<BreweryRequest, Brewery>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BreweryBeers, opt => opt.Ignore()) //Ignore if it's not part 
                .ReverseMap();
            //CreateMap<Brewery, BreweryRequest>()
            //    .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => new BeerRequest
            //    {
            //        Id = bb.Beer.Id,
            //        Name = bb.Beer.Name,
            //        PercentageAlcoholByVolume = bb.Beer.PercentageAlcoholByVolume,
            //    }).ToList()))
            //    .ReverseMap();
            CreateMap<Brewery, BreweryResponse>();

            // BarBeerLink Mappings
            CreateMap<BarBeerLink, BarBeerLinkRequest>();
            // BreweryBeerLink Mappings
            CreateMap<BreweryBeerLink, BreweryBeerLinkRequest>();


        }
    }
}
