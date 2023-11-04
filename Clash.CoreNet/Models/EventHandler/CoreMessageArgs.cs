using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class CoreLogData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("payload")] 
    public string Message { get; set; }
}
