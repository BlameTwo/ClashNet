using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Interfaces;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Clash.CoreNet.Helper;

/// <summary>
/// 格式化各种代理协议的类
/// </summary>
public static class ProxyFormatHelper
{
    /// <summary>
    /// VMess协议
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static IProxyItem FormatVmess(string line)
    {
        if (!line.StartsWith("vmess://")) return null;
        string vmessstr = line.Substring(line.LastIndexOf("/")+1);
        var base64 =Encoding.UTF8.GetString( Convert.FromBase64String(vmessstr));
        var jobject = JsonObject.Parse(base64)!;
        VmessProxyItem item = new();
        item.Server = jobject["add"]!.GetValue<string>();
        item.alterId = "auto";
        item.udp = true;
        item.uuid = jobject["id"]!.GetValue<string>()!;
        item.alterId = jobject["aid"].GetValue<string>();
        item.Name = jobject["ps"].GetValue<string>().Replace("#", "");
        item.Port = jobject["port"].GetValue<string>();
        var istls = jobject["tls"].GetValue<string>();
        if (!string.IsNullOrWhiteSpace(istls))
            item.tls = true;
        item.ws_path = jobject["path"].GetValue<string>();
        item.Ws_header = new()
        {
            Host = jobject["host"].GetValue<string>()
        };
        item.WS_Opts = new()
        {
            Headers = new()
            {
                Host = jobject["host"].GetValue<string>()
            },
            Path = item.ws_path
        };
        item.cipher = "auto";
        item.network = jobject["net"].GetValue<string>();
        return item;
    }


    /// <summary>
    /// 来自于Url的更新订阅
    /// </summary>
    /// <param name="SessionUrl">代理服务商提供的地址</param>
    /// <returns>返回一个IProxyItem集合的IP代理列表</returns>
    public static async Task<List<IProxyItem>> UpdateFormUrl(string SessionUrl)
    {
        string pattern = @"^(http|https|ftp)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        bool isMatch = Regex.IsMatch(SessionUrl, pattern);
        if (!isMatch)
            throw new Exception("格式不为Url");
        List<IProxyItem> list = new List<IProxyItem>();
        var base64str = (await HttpClientHelper.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri(SessionUrl),
            Method = HttpMethod.Get
        })).StreamToString();
        var basebyte = Convert.FromBase64String(base64str);
        var linestr = Encoding.UTF8.GetString(basebyte);
        var linelist = linestr.Split("\r\n");
        foreach (var line in linelist)
        {
            if (line.StartsWith("vmess://"))
            {
                list.Add(ProxyFormatHelper.FormatVmess(line));
            }
            else if (line.StartsWith("trojan://"))
            {
                list.Add(ProxyFormatHelper.FormatTrojan(new(line)));
            }
            else if (line.StartsWith("ss://"))
            {
                list.Add(ProxyFormatHelper.FormatSS(new(line)));
            }
        }
        return list;
    }

    /// <summary>
    /// Trojan协议
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static IProxyItem FormatTrojan(Uri uri)
    {
        TrojanProxyItem item = new();
        item.Password = uri.UserInfo;
        item.Name = System.Web.HttpUtility.UrlDecode(uri.Fragment,Encoding.UTF8).Replace("#","");
        item.Port = uri.Port.ToString();
        item.Server = uri.Host;
        item.Sni = uri.IdnHost;
        item.Udp = true;
        return item;
    }

    /// <summary>
    /// 处理SS协议
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static IProxyItem FormatSS(Uri uri)
    {
        string base64 = uri.UserInfo.Base64Error();
        string originalString = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        SSProxyItem item = new();
        item.Password = originalString.Substring(originalString.IndexOf(":")+1);
        item.cid = originalString.Substring(0,originalString.IndexOf(":"));
        item.Name = System.Web.HttpUtility.UrlDecode(uri.Fragment, Encoding.UTF8).Replace("#", "");
        item.Port = uri.Port.ToString();
        item.Server = uri.Host;
        item.Udp = true;
        return item;
    }
}
