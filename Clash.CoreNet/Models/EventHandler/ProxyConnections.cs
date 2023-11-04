using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.EventHandler;

public class ProxyConnections
{
    [JsonPropertyName("downloadTotal")]public long DownloadTotal { get; set; }

    [JsonPropertyName("uploadTotal")]public long UploadTotal { get; set; }

    [JsonPropertyName("connections")]public List<Connection> Connections { get; set; }  
}

public class Connection
{
    [JsonPropertyName("id")]public string ID { get; set; }

    [JsonPropertyName("upload")]public long Upload { get; set; }

    [JsonPropertyName ("download")]public long Download { get; set; }

    [JsonPropertyName("start")]public DateTime Start { get; set; }

    [JsonPropertyName("rule")]public string Rule { get; set; }

    [JsonPropertyName("rulePayload")]public string RulePayload { get; set; }

    [JsonPropertyName("chains")]public List<string> Chains { get; set; }

    [JsonPropertyName("metadata")]public MetaData MetaData { get; set; } 
}

public class MetaData
{

    [JsonPropertyName("network")]public string NetWork { get; set; }

    [JsonPropertyName("type")]public string Type { get; set; }

    [JsonPropertyName("sourceIP")]public string SourceIP { get; set; }

    [JsonPropertyName("destinationIP")]public string Destnation { get; set; }

    [JsonPropertyName("sourcePort")]public string SourcePort { get; set; }

    [JsonPropertyName("destinationPort")]public string DestinationIP { get; set; }

    [JsonPropertyName("host")]public string Host { get; set; }

    [JsonPropertyName("dnsMode")]public string DnsMode { get; set; }

    [JsonPropertyName("processPath")]public string ProcessPath { get; set; }

    [JsonPropertyName("specialProxy")]public string SpecialProxy { get; set; }
}