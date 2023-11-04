using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;
using YamlDotNet.Serialization;

namespace Clash.CoreNet.Models;

/// <summary>
/// VMess协议
/// </summary>
public class VmessProxyItem : IProxyItem
{
    [YamlMember(Alias ="name")]
    public string Name { get; set; }

    [YamlMember(Alias = "port")]
    public string Port { get; set; }

    [YamlMember(Alias = "server")]
    public string Server { get; set; }

    [YamlMember(Alias = "uuid")]
    public string uuid { get; set; }

    [YamlMember(Alias = "alterId")]
    public string alterId { get; set; }

    [YamlMember(Alias = "tls")]
    public bool? tls { get; set; }

    [YamlMember(Alias = "udp")]
    public bool udp { get; set; }


    [YamlMember(Alias = "network")]
    public string network { get; set; }

    [YamlMember(Alias = "cipher")]
    public string cipher { get; set; }


    [YamlMember(Alias = "ws-path")]
    public string ws_path { get; set; }


    [YamlMember(Alias = "ws-headers")]
    public VmessHeader Ws_header { get; set; }

    [YamlMember(Alias = "ws-opts")]
    public VmessWSOpts WS_Opts { get; set; }

    [YamlMember(Alias = "type")]
    public ProxyItemEnum Type { get; set; } = ProxyItemEnum.vmess;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class VmessWSOpts
{
    [YamlMember(Alias = "path")]
    public string Path { get; set; }

    [YamlMember(Alias = "headers")]
    public VmessHeader Headers { get; set; }
}


/// <summary>
/// Trojan协议
/// </summary>
public class TrojanProxyItem : IProxyItem
{

    [YamlMember(Alias ="password")]
    public string Password { get; set; }

    [YamlMember(Alias = "sni")]
    public string Sni { get; set; }

    [YamlMember(Alias = "udp")]
    public bool Udp { get; set; } = true;


    [YamlMember(Alias = "name")]
    public string Name { get; set; }


    [YamlMember(Alias = "port")]
    public string Port { get; set; }

    [YamlMember(Alias = "server")]
    public string Server { get; set; }

    [YamlMember(Alias ="type")]
    public ProxyItemEnum Type { get; set; } = ProxyItemEnum.trojan;
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

/// <summary>
/// VmessHeader
/// </summary>
public class VmessHeader
{
    [YamlMember(Alias ="Host")]
    public string Host { get; set; }
}

/// <summary>
/// ss协议
/// </summary>
public class SSProxyItem:IProxyItem
{
    [YamlMember(Alias = "cipher")]
    public string cid { get; set; }

    [YamlMember(Alias = "password")]
    public string Password { get; set; }

    [YamlMember(Alias = "udp")]
    public bool Udp { get; set; }

    [YamlMember(Alias ="name")]
    public string Name { get; set; }

    [YamlMember(Alias = "port")]
    public string Port { get; set; }

    [YamlMember(Alias = "server")]
    public string Server { get; set; }

    [YamlMember(Alias = "type")]
    public ProxyItemEnum Type { get; set; } = ProxyItemEnum.ss;
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}