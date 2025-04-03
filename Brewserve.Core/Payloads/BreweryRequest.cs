using System.Text.Json.Serialization;
namespace BrewServe.Core.Payloads;
public class BreweryRequest
{
    [JsonIgnore] 
    public int?Id { get; set; }
    public string Name { get; set; }
}