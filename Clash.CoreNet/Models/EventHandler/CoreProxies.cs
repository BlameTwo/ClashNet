using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class CoreProxies
{
    [JsonPropertyName("proxies")]
    [JsonConverter(typeof(CoreProxiesConverter))]
    public List<CoreProxiesItem> Items { get; set; }
}

public class CoreProxiesItem
{
    [JsonPropertyName("history")]
    public List<ProxiesHistory> History { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("udi")]
    public bool Udp { get; set; }

    [JsonPropertyName("all")]
    public List<string> Allproxies { get; set; }

    [JsonPropertyName("now")]
    public string NowSelectName { get; set; }
}

public class CoreProxiesConverter : JsonConverter<List<CoreProxiesItem>>
{
    public override List<CoreProxiesItem>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<CoreProxiesItem> list = new();
        var job = JsonObject.Parse(ref reader);
        var item  =  job.AsObject();
        foreach (var it in item)
        {
            var result = (it.Value.AsObject()).Deserialize<CoreProxiesItem>() ;
            list.Add(result);
        }
        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<CoreProxiesItem> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

public class ProxiesHistory
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("delay")]
    public int Delay { get; set; }

    [JsonPropertyName("meanDelay")]
    public int MeanDelay { get; set; }
}
