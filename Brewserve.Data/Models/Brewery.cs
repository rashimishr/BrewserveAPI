using System.Diagnostics.CodeAnalysis;

namespace BrewServe.Data.Models;

[ExcludeFromCodeCoverage]
public class Brewery
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public ICollection<BreweryBeerLink> BreweryBeers { get; set; } = new List<BreweryBeerLink>();
}