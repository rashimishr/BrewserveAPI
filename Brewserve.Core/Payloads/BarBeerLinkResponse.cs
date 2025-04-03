namespace BrewServe.Core.Payloads;
public class BarBeerLinkResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<BeerResponse> Beers { get; set; }
}