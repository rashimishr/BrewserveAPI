
namespace Brewserve.Core.DTOs
{
    public class CreateBeerDTO
    {
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
    public class BeerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
        public List<BreweryDTO> Breweries { get; set; }
    }
}
