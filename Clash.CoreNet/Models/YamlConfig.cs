using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;
using YamlDotNet.Serialization;

namespace Clash.CoreNet.Models;

public class Config : ICloneable
{
    /// <summary>
    /// 唯一Guid，注意这个Guid只存在于已经缓存好的配置文件，记住一定要在导出Yaml配置文件时将这个属性定义为null
    /// </summary>
    [YamlMember(Alias ="Guid")]
    public string Guid { get; set; }

    /// <summary>
    /// 更新Url
    /// </summary>
    [YamlMember(Alias ="UpdateUrl")]
    public string UpdateUrl { get; set; }


    [YamlMember(Alias ="LastUpdate")]
    public DateTime? UpdateTime { get; set; }


    /// <summary>
    /// 临时缓存文件的备注名称
    /// </summary>
    [YamlMember(Alias ="Name")]
    public string Name { get; set; }

    [YamlMember(Alias ="UA")]
    public string UA { get; set; }


    [YamlMember(Alias = "ipv6")]
    public bool ipv6 { get; set; }

    [YamlMember(Alias = "port")]
    public int Port { get; set; }

    [YamlMember(Alias = "mixed-port")]
    public int MixedPort { get; set; }

    [YamlMember(Alias = "bind-address")]
    public string BindAddress { get; set; }

    [YamlMember(Alias = "socks-port")]
    public int socks_port { get; set; }


    [YamlMember(Alias = "allow-lan")]
    public bool allow_lan { get; set; }

    [YamlMember(Alias = "mode")]
    public RuleMode mode { get; set; }

    [YamlMember(Alias = "log-level")]
    public string log_level { get; set; }

    [YamlMember(Alias = "external-controller")]
    public string external_controller { get; set; }

    [YamlMember(Alias = "secret")]
    public string secret { get; set; }

    [YamlMember(Alias = "dns")]
    public Dns dns { get; set; }

    [YamlMember(Alias = "proxies")]
    public List<IProxyItem> proxies { get; set; } = new();


    [YamlMember(Alias = "proxy-groups")]
    public List<ProxyGroup> proxy_groups { get; set; } = new();

    [YamlMember(Alias = "rules")]
    public List<string> rules { get; set; }

    /// <summary>
    /// 克隆
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

// 定义DNS的类
public class Dns
{
    [YamlMember(Alias = "enable")]
    public bool enable { get; set; }


    [YamlMember(Alias = "listen")]
    public string listen { get; set; }

    [YamlMember(Alias = "default-nameserver")]
    public List<string> default_nameserver { get; set; } = new();


    [YamlMember(Alias = "enhanced-mode")]
    public string enhanced_mode { get; set; }

    [YamlMember(Alias = "ipv6")]
    public bool IPv6 { get; set; }

    [YamlMember(Alias = "fake-ip-range")]
    public string fake_ip_range { get; set; }

    [YamlMember(Alias = "use-hosts")]
    public bool use_hosts { get; set; }

    [YamlMember(Alias = nameof(nameserver))]
    public List<string> nameserver { get; set; } = new();

    [YamlMember(Alias = nameof(fallback))]
    public List<string> fallback { get; set; } = new();


    [YamlMember(Alias = "fallback-filter")]
    public FallbackFilter fallback_filter { get; set; } = new();
}

public class FallbackFilter
{
    [YamlMember(Alias = "geoip")]
    public bool Geoip { get; set; } // 是否通过 GEOIP 来判断是否需要 fallback 列表中额外检测 DNS 响应结果。

    [YamlMember(Alias = "geoip-code")]
    public string GeoipCode { get; set; } // GEOIP 的国家代码

    [YamlMember(Alias = "ipcidr")]
    public List<string> Ipcidr { get; set; } = new(); // IP 地址段列表，用于判断 DNS 响应结果是否可信
}

// 定义代理节点的类
public class Proxy
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "type")]
    public string Type { get; set; }


    [YamlMember(Alias = "server")]
    public string Server { get; set; }


    [YamlMember(Alias = "port")]
    public int Port { get; set; }

    [YamlMember(Alias = "password")]
    public string Password { get; set; }


    [YamlMember(Alias = "sni")]
    public string Sni { get; set; }

    [YamlMember(Alias = "udp")]
    public bool udp { get; set; }
}

// 定义代理策略组的类
public class ProxyGroup
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }


    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "proxies")]
    public List<string> Proxies { get; set; } = new();

    [YamlMember(Alias = "url")]
    public string url { get; set; }

    [YamlMember(Alias = "interval")]
    public string Interval { get; set; }
}

public class Rule
{
    public string Type { get; set; } // 规则类型，如DOMAIN-SUFFIX, GEOIP, MATCH等
    public string Pattern { get; set; } // 规则匹配模式，如google.com, CN等
    public string Policy { get; set; } // 规则使用的策略组，如Proxy, DIRECT等
}
