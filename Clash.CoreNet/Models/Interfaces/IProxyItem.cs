using Clash.CoreNet.Models.Enums;

namespace Clash.CoreNet.Models.Interfaces;

/// <summary>
/// 代理IP的接口
/// </summary>
public interface IProxyItem:ICloneable
{
    /// <summary>
    /// 代理IP的名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 代理IP的端口
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// 代理服务器的地址
    /// </summary>
    public string Server { get; set; }

    public ProxyItemEnum Type { get; set; }

}
