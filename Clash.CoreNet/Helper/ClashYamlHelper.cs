using Clash.CoreNet.Converter;
using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Clash;
using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;
using System.Diagnostics;
using System.IO;
using YamlDotNet.Serialization;

namespace Clash.CoreNet.Helper;

/// <summary>
/// 生成配置文件
/// </summary>
public static class ClashYamlHelper
{


    static ProxyItemConvert itemconvert = new();

    static async Task<RuleFormatData> Make(string url)
    {
        var request = await HttpClientHelper.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        });
        var rulesyaml = request.StreamToString();
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<RuleFormatData>(rulesyaml);

    }

    /// <summary>
    /// 增加一个自己的代理规则
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task MakeMyYaml(string url)
    {
        await Make(url);
    }

    /// <summary>
    /// 更新配置文件
    /// </summary>
    /// <param name="path">原地址</param>
    /// <param name="guidstr">guid标识符</param>
    /// <returns></returns>
    public static async Task<Tuple<bool,string>> UpdateConfig(string path,string guidstr)
    {
        var builder = ClashYamlHelper.GetDefaultYamlDeserializerBuilder().Build();
        var builder2 = ClashYamlHelper.GetDefaultYamlSerializerBuilder().Build();
        var filetext = await File.ReadAllTextAsync(path);
        Config config = builder.Deserialize<Config>(filetext);
        if (config.Guid != guidstr || string.IsNullOrWhiteSpace(config.UpdateUrl)) 
            return new(false, "Guid不匹配或更新Url不存在，请删除配置文件进行新增更新");
        var result = await ProxyFormatHelper.UpdateFormUrl(config.UpdateUrl);
        config.proxies = null;
        config.proxies = result;
        config.proxy_groups = null;
        config.UpdateTime = DateTime.Now;
        config.proxy_groups = MakeDefaultProxyGroup(result);
        var yamlstr = builder2.Serialize(config);
        await File.WriteAllTextAsync(path, yamlstr);
        return new Tuple<bool, string>(true,$"更新{config.Name}成功！请刷新配置文件目录");
    }

    /// <summary>
    /// 创建默认的代理分组规则
    /// </summary>
    /// <param name="ipitems">代理列表</param>
    /// <returns></returns>
    public static  List<ProxyGroup> MakeDefaultProxyGroup(List<IProxyItem> ipitems)
    {
        
        List<ProxyGroup> groups = null;
        var urltest = new ProxyGroup()
        {
            Type = "url-test",
            Name = "自动选择",
            url = AppProperty.GroupTestUrl,
            Interval = AppProperty.Url_TestPing.ToString()
        };
        var fallback = new ProxyGroup()
        {
            Type = "fallback",
            Name = "故障转移",
            url = AppProperty.GroupTestUrl,
            Interval = AppProperty.FallbackPing.ToString()
        };
        var selectgroup = new ProxyGroup()
        {
            Type = "select",
            Name = "手动选择"
        };
        ipitems.ForEach((val) =>
        {
            urltest.Proxies.Add(val.Name);
            fallback.Proxies.Add(val.Name);
            selectgroup.Proxies.Add(val.Name);
        });
        selectgroup.Proxies.Add("自动选择");
        selectgroup.Proxies.Add("故障转移");
        groups = new List<ProxyGroup> { urltest, fallback, selectgroup };
        return groups;
    }

    /// <summary>
    /// 创建默认DNS配置
    /// </summary>
    /// <returns></returns>
    public static Dns MakeDefaultDns()
    {
        Dns dns = new Dns();
        dns.enable = true;
        dns.default_nameserver.Add("224.5.5.5");
        dns.default_nameserver.Add("119.29.29.29");
        dns.enhanced_mode = "fake-ip";
        dns.IPv6 = false;
        dns.fake_ip_range = "192.168.0.1/16";
        dns.use_hosts = true;
        dns.nameserver.Add("https://doh.pub/dns-query");
        dns.nameserver.Add("https://dns.alidns.com/dns-query");
        dns.fallback.Add("https://doh.dns.sb/dns-query");
        dns.fallback.Add("https://dns.cloudflare.com/dns-query");
        dns.fallback.Add("https://dns.twnic.tw/dns-query");
        dns.fallback.Add("tls://8.8.4.4:853");
        dns.fallback_filter.Geoip = true;
        dns.fallback_filter.Ipcidr.Add("240.0.0.0/4");
        dns.fallback_filter.Ipcidr.Add("0.0.0.0/32");
        return dns;
    }


    /// <summary>
    /// 根据代理IP生成配置文件，默认是直连模式
    /// </summary>
    /// <param name="lists">根据<see cref="UpdateFormUrl(string)"/>方法获得IP数组</param>
    /// <param name="config">Core配置文件，可空</param>
    /// <param name="rulemod">代理模式</param>
    /// <returns>返回一个元组，第一个项目是配置文件，第二个是已经保存的配置文件地址</returns>
    public static async Task<Tuple<Config, string>> MakeConfig(MakeConfigData data,string url)
    {
        string guid = GuidHelper.GetNewGuid().ToString();
        ProxyConfig config = new();
        Config yamlconfig = new();
        yamlconfig.Guid = guid;
        yamlconfig.UpdateTime = DateTime.Now;
        yamlconfig.UpdateUrl = url;
        yamlconfig.Name = data.name;
        yamlconfig.UA = data.UA;
        yamlconfig.Port = config.Port;
        yamlconfig.MixedPort = config.MixedPort;
        yamlconfig.ipv6 = false;
        yamlconfig.socks_port = config.SocksPort;
        yamlconfig.allow_lan = config.OpenLan;
        yamlconfig.mode = data.RuleMode;
        yamlconfig.log_level = LogLevel.info.ToString();
        yamlconfig.external_controller ="127.0.0.1:" + config.APIPort;
        yamlconfig.proxies = data.items;
        #region 创建默认的代理分组
        yamlconfig.proxy_groups = ClashYamlHelper.MakeDefaultProxyGroup(data.items);
        #endregion
        #region 配置解析规则，使用自动选择分组
        yamlconfig.rules =await RuleHelper.MakeRule(Rules.GetDefaultProxyRulesUrl(),"自动选择");
        #endregion
        yamlconfig.dns = ClashYamlHelper.MakeDefaultDns();

        var builder = new SerializerBuilder()
            .WithEventEmitter(nextEmitter => new QuoteStringValueEmitter(nextEmitter))
            .WithTypeConverter(itemconvert)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();
        var yaml = builder.Serialize(yamlconfig);
        FileHelper.FolderExits(AppProperty.AppClashCacheFolder);
        var path = Path.Combine(AppProperty.AppClashCacheFolder, $"{guid}.yaml");
        await File.WriteAllTextAsync(path, yaml);
        return new Tuple<Config, string>(yamlconfig, path);
    }

    /// <summary>
    /// 生成不包含代理IP和任何规则的配置文件，默认是直连模式
    /// </summary>
    /// <param name="config">Core配置文件</param>
    /// <returns>返回一个元组，第一个项目是配置文件，第二个是已经保存的配置文件地址</returns>
    public static async Task<Tuple<Config, string>> MakeConfig(string name)
    {
        string guid= GuidHelper.GetNewGuid().ToString();
        ProxyConfig config   = new();
        Config yamlconfig = new();
        yamlconfig.Guid = guid;
        yamlconfig.UpdateTime = DateTime.Now;
        Random random = new Random(1000);
        if (!string.IsNullOrWhiteSpace(name)) yamlconfig.Name = name;
        else  yamlconfig.Name = $"RandomFile{random.Next().ToString()}";
        yamlconfig.Port = config.Port;
        yamlconfig.external_controller = "127.0.0.1:" + config.APIPort;
        yamlconfig.MixedPort = config.MixedPort;
        yamlconfig.ipv6 = false;
        yamlconfig.socks_port = config.SocksPort;
        yamlconfig.allow_lan = config.OpenLan;
        yamlconfig.mode = RuleMode.Direct;
        yamlconfig.log_level = LogLevel.info.ToString();
        yamlconfig.dns = ClashYamlHelper.MakeDefaultDns();
        var builder = new SerializerBuilder()
            .WithEventEmitter(nextEmitter => new QuoteStringValueEmitter(nextEmitter))
            .WithTypeConverter(itemconvert)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();
        var yaml = builder.Serialize(yamlconfig);
        FileHelper.FolderExits(AppProperty.AppClashCacheFolder);
        var path = Path.Combine(AppProperty.AppClashCacheFolder, $"{guid}.yaml");
        await File.WriteAllTextAsync(path, yaml);
        return new Tuple<Config, string>(yamlconfig, path);
    }


    public static SerializerBuilder GetDefaultYamlSerializerBuilder()
    {
        return new SerializerBuilder()
            .WithEventEmitter(nextEmitter => new QuoteStringValueEmitter(nextEmitter))
            .WithTypeConverter(itemconvert)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull);
    }

    public static DeserializerBuilder GetDefaultYamlDeserializerBuilder()
    {
        return new DeserializerBuilder()
            .WithTypeConverter(itemconvert);
    }

    /// <summary>
    /// 从缓存配置文件中导出到正式文件中
    /// </summary>
    /// <param name="config">配置文件</param>
    /// <param name="fileconfig">切换文件配置</param>
    /// <returns></returns>
    internal async static Task<bool> ToClashFile(Config config, ToClashFileData fileconfig)
    {
        try
        {
            Config config2 = new();
            config2 = (Config)config.Clone();
            if (File.Exists(AppProperty.ClashConfigFolder + "\\config.yaml"))
                File.Delete(AppProperty.ClashConfigFolder + "\\config.yaml");
            config2.mode = fileconfig.rulemod;
            var builder = ClashYamlHelper.GetDefaultYamlSerializerBuilder().Build();
            //Guid设置为空
            config2.Guid = null;
            config2.Name = null;
            config2.UA = null;
            config2.UpdateUrl = null;
            config2.UpdateTime = null;
            var yamlstr = builder.Serialize(config2);
            var flage = await FileHelper.FileExits(AppProperty.ClashConfigFolder + "\\config.yaml", true);
            await File.WriteAllTextAsync(AppProperty.ClashConfigFolder + "\\config.yaml", yamlstr);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


}
