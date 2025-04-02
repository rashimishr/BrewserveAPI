
using Brewserve.Data.Models;

namespace Brewserve.Core.Payloads
{
    public class CreateBeerRequest
    {
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
    public class BeerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
        public List<BreweryRequest> Breweries { get; set; }
    }
}
