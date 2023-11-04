namespace Clash.CoreNet;

/// <summary>
/// 核心属性
/// </summary>
public static class AppProperty
{
    static string AppBaseFolder = System.AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// AppConfig目录
    /// </summary>
    public static string AppConfigFolder = AppBaseFolder +"AppConfig";

    /// <summary>
    /// 下载文件缓存文件夹
    /// </summary>
    public static string CacheFolder = AppBaseFolder + "CacheFile";

    /// <summary>
    /// 多个配置文件的缓存目录
    /// </summary>
    public static string AppClashCacheFolder = AppBaseFolder + "\\clashYaml";

    /// <summary>
    /// Clash保存目录和Config.yaml配置文件地址
    /// </summary>
    public static string ClashConfigFolder = AppBaseFolder + "Clash";

    public static string ClashConfigJsonFile = AppBaseFolder + "Clash\\Clashconfig.json";

    /// <summary>
    /// 分组IP测试地址
    /// </summary>
    public const string GroupTestUrl = "http://www.gstatic.com/generate_204";

    public const int ProxyItemPing = 3500;

    public const string ProxyItemTestUrl = "https://www.google.com/generate_204";

    /// <summary>
    /// 自动选择测试ping
    /// </summary>
    public const int Url_TestPing = 500;

    /// <summary>
    /// 故障转义测试ping
    /// </summary>
    public const int FallbackPing = 1000;

    /// <summary>
    /// Clash核心更新地址
    /// </summary>
    public const string ClashCoreUpdateUrl = "https://api.github.com/repos/Dreamacro/clash/releases/latest";
}
