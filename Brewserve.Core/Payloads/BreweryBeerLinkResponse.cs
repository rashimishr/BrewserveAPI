namespace BrewServe.Core.Payloads;

public class BreweryBeerLinkResponse
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public List<BeerResponse> Beers { get; set; }
}