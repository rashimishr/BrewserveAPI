namespace BrewServe.Data.Models;
public class BarBeerLink
{
    public int BarId { get; set; }
    public Bar Bar { get; set; }
    public int BeerId { get; set; }
    public Beer Beer { get; set; }
}