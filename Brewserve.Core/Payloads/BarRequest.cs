using System.Text.Json.Serialization;
namespace BrewServe.Core.Payloads;
public class BarRequest
{
    [JsonIgnore] 
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}