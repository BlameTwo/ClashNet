using Clash.CoreNet.Models.Enums;

namespace Clash.CoreNet;

/// <summary>
/// 自动解析组
/// </summary>
public class Rules
{
    /*
     使用https://gitmirror.com/raw.html进行加速下载服务
     */

   /// <summary>
   /// 谷歌PCM单独信息
   /// </summary>
    public static readonly string GooglePCM = "https://raw.staticdn.net/ACL4SSR/ACL4SSR/master/Clash/Ruleset/GoogleFCM.list";
    /// <summary>
    /// Steam单独信息
    /// </summary>
    public static readonly string SteamCN = "https://raw.staticdn.net/ACL4SSR/ACL4SSR/master/Clash/Ruleset/SteamCN.list";
    /// <summary>
    /// 微软单独信息
    /// </summary>
    public static readonly string MicrosoftCN = "https://raw.gitmirror.net/ACL4SSR/ACL4SSR/master/Clash/Microsoft.list";
    /// <summary>
    /// Telegram单独信息
    /// </summary>
    public static readonly string TelegramCN = "https://raw.gitmirror.net/ACL4SSR/ACL4SSR/master/Clash/Telegram.list";
    /// <summary>
    /// 全球媒体单独信息
    /// </summary>
    public static readonly string EarthMedia = "https://raw.gitmirror.net/ACL4SSR/ACL4SSR/master/Clash/ProxyMedia.list";

    /// <summary>
    /// 包含以上大部分的代理信息
    /// </summary>
    public static readonly string ProxyCN = "https://raw.staticdn.net/ACL4SSR/ACL4SSR/master/Clash/ProxyLite.list";


    /// <summary>
    /// 全球直连单独信息
    /// </summary>
    public static readonly string EarthIP = "https://raw.githubusercontent.com/ACL4SSR/ACL4SSR/master/Clash/ProxyLite.list";


    public static List<string> GetDefaultProxyRulesUrl() => new List<string>() 
    {
        ProxyCN
    };
}
