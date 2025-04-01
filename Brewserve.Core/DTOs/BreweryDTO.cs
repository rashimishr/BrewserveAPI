
namespace Brewserve.Core.DTOs
{
    public class CreateBreweryDTO
    {
        public string Name { get; set; }
    }
    public class BreweryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BeerDTO> Beers { get; set; }
    }
}
