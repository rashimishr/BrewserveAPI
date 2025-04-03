using System.Text.Json.Serialization;

namespace BrewServe.Core.Payloads
{
 public class BeerRequest
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
}
