using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class ProxyDelay
{
    [JsonPropertyName("delay")]
    public int Delay { get; set; }

    [JsonPropertyName("meanDelay")]
    public int MeanDelay { get; set; }
}
