using System.Diagnostics.CodeAnalysis;

namespace Brewserve.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class Bar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<BarBeerLink> BarBeers { get; set; } = new List<BarBeerLink>();
    }
}
