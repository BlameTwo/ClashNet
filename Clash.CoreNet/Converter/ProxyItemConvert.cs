using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Clash.CoreNet.Converter;

public class ProxyItemConvert : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(IProxyItem) 
        || type == typeof(TrojanProxyItem)
        || type == typeof(VmessProxyItem)
        ||type == typeof(SSProxyItem);

    public object? ReadYaml(IParser parser, Type type)
    {
        IProxyItem? proxyitem=null;
        object itemtype = "";
        var deserializer = new DeserializerBuilder().Build();
        Dictionary<object,object> yamlObject = deserializer.Deserialize<Dictionary<object, object>>(parser);
        yamlObject.TryGetValue("type", out itemtype);
        switch (itemtype!.ToString())
        {
            case "trojan":
                {
                    var result = FormatTrojan(yamlObject);
                    return result;
                }
            case "vmess":
                {
                    var result = FormatVmess(yamlObject);
                    return result;
                }
                case "ss":
                {
                    var result = FormatSS(yamlObject);
                    return result;
                }
        }
        throw new YamlException($"不支持的{itemtype!.ToString()}协议");
    }


    /// <summary>
    /// 转换Trojan协议
    /// </summary>
    /// <param name="dictvalues"></param>
    /// <returns></returns>
    IProxyItem FormatTrojan(Dictionary<object, object> dictvalues)
    {
        var proxy = new TrojanProxyItem();
        proxy.Sni = dictvalues["sni"].ToString()!;
        proxy.Udp = Convert.ToBoolean(dictvalues["udp"]);
        proxy.Type = ProxyItemEnum.trojan;
        proxy.Name = dictvalues["name"].ToString()!;
        proxy.Server = dictvalues["server"].ToString()!;
        proxy.Password = dictvalues["password"].ToString()!;
        proxy.Port = dictvalues["port"].ToString()!;
        return proxy;
    }

    /// <summary>
    /// 转换Vmess协议
    /// </summary>
    /// <param name="dictvalues"></param>
    /// <returns></returns>
    IProxyItem FormatVmess(Dictionary<object,object> dictvalues)
    {
        VmessProxyItem proxy = new();
        object objheaders = new();
        dictvalues.TryGetValue("ws-headers", out objheaders);
        try
        {
            proxy.Name = dictvalues["name"].ToString()!;
            proxy.Server = dictvalues["server"].ToString()!;
            proxy.alterId = dictvalues["alterId"].ToString()!;
            proxy.Port = dictvalues["port"].ToString()!;
            proxy.network = dictvalues["network"].ToString()!;
            proxy.cipher = dictvalues["cipher"].ToString()!;
            if (dictvalues.ContainsKey("tls"))
                proxy.tls = true;
            else
                proxy.tls = null;
            proxy.ws_path = dictvalues["ws-path"].ToString()!;
            proxy.Ws_header = new() { Host = (objheaders as Dictionary<object, object>)!["Host"].ToString()! };
            var opts = new VmessWSOpts();
            object optsobject = null;
            dictvalues.TryGetValue("ws-opts", out optsobject);
            var optsvalue = optsobject as Dictionary<object,object>;
            opts.Path= optsvalue!["path"].ToString()!;
            opts.Headers = new() { Host = (optsvalue["headers"] as Dictionary<object, object>)!["Host"].ToString()! };
            proxy.WS_Opts = opts;
            proxy.uuid = dictvalues["uuid"].ToString()!;
            return proxy;
        }
        catch (Exception ex)
        {
            return proxy;
        }
    }

    /// <summary>
    /// 生成ss协议
    /// </summary>
    /// <param name="dictvalues"></param>
    /// <returns></returns>
    IProxyItem FormatSS(Dictionary<object, object> dictvalues)
    {
        SSProxyItem proxy = new();
        proxy.Name = dictvalues["name"].ToString()!;
        proxy.Server = dictvalues["server"].ToString()!;
        proxy.Port = dictvalues["port"].ToString()!;
        proxy.cid = dictvalues["cipher"].ToString()!;
        proxy.Password = dictvalues["password"].ToString()!;
        proxy.Udp = Convert.ToBoolean(dictvalues["udp"]) ;
        return proxy;   
    }


    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        var valueSerializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .BuildValueSerializer();
        if (type == typeof(VmessProxyItem))
        {
            VmessProxyItem item = (VmessProxyItem)value;
            valueSerializer.SerializeValue(emitter, item, typeof(VmessProxyItem));
        }
        if (type == typeof(TrojanProxyItem))
        {
            TrojanProxyItem item = (TrojanProxyItem)value;
            valueSerializer.SerializeValue(emitter, item, typeof(TrojanProxyItem));
        }
        if(type == typeof(SSProxyItem))
        {
            SSProxyItem item = (SSProxyItem)value;
            valueSerializer.SerializeValue(emitter, item, typeof(SSProxyItem));
        }
    }
}
