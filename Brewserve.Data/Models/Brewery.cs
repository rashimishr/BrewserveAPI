namespace Brewserve.Data.Models
{
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BreweryBeerLink> BreweryBeers { get; set; } = new List<BreweryBeerLink>();
    }
}
