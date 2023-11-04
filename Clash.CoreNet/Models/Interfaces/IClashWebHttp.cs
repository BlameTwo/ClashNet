using Clash.CoreNet.Models.EventHandler;
using static Clash.CoreNet.ClashWebHttp;

namespace Clash.CoreNet.Models.Interfaces;

public interface IClashWebHttp
{
    public string BaseAddress { get; set; }
    public Task<IsRunCore> GetCoreRun();

    public Task InitMsg();
    public Task<CoreLogData> GetCoreLog();

    public Task<CoreProxies> GetProxies();

    public Task<string> ChangSelectProxy(string groupname, string newproxyname);

    public Task<ProxyConnections> GetConnections();

    public Task<string> DelectConnection(string id);
}

