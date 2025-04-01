
namespace Brewserve.Core.DTOs
{
    public class CreateBarDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class BarDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<BeerDTO> Beers { get; set; }
    }
}
