
namespace Brewserve.Core.Payloads
{
    public class CreateBreweryRequest
    {
        public string Name { get; set; }
    }
    public class BreweryRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BeerRequest> Beers { get; set; }
    }
}
