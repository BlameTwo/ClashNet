using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class IsRunCore
{
    [JsonPropertyName("hello")]
    public string Hello { get; set; }
}
