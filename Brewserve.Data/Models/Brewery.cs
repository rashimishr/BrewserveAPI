using System.Diagnostics.CodeAnalysis;

namespace Brewserve.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<BreweryBeerLink> BreweryBeers { get; set; } = new List<BreweryBeerLink>();
    }
}
