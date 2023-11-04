using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class CoreSpendArgs
{
    [JsonPropertyName("down")]
    public long Down { get; set; }

    [JsonPropertyName("up")]
    public long Up { get; set; }
}
