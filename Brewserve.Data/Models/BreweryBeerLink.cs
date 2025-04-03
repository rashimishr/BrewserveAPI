namespace BrewServe.Data.Models;

public class BreweryBeerLink
{
    public int BreweryId { get; set; }
    public Brewery Brewery { get; set; }
    public int BeerId { get; set; }
    public Beer Beer { get; set; }
}