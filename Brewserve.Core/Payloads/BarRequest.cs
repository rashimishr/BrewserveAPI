
namespace Brewserve.Core.Payloads
{
    public class CreateBarRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class BarRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<BeerRequest> Beers { get; set; }
    }
}
