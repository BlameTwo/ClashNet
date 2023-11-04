using Clash.CoreNet.Models.Enums;
using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.Clash
{
    public class ChangedClashConfig
    {
        [JsonPropertyName("port")]
        public int? port { get; set; } = null;

        [JsonPropertyName("socks-port")]
        public int? socksport { get; set; } = null;


        [JsonPropertyName("redier-port")]
        public string? RedierPort { get; set; } = null;

        [JsonPropertyName("mode")]
        public string? mode { get; set; } = null;


        [JsonPropertyName("log-level")]
        public string loglevel { get; set; } = null;

        [JsonPropertyName("allow-lan")]
        public bool? AllowLan { get; set; } = null;


    }
}
