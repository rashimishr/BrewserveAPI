using System.Diagnostics.CodeAnalysis;
namespace BrewServe.Data.Models;
[ExcludeFromCodeCoverage]
public class Beer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal PercentageAlcoholByVolume { get; set; }
    public ICollection<BarBeerLink> BarBeers { get; set; } = new List<BarBeerLink>();
    public ICollection<BreweryBeerLink> BreweryBeers { get; set; } = new List<BreweryBeerLink>();
}