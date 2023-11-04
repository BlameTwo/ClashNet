using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Clash;
using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;

namespace Clash.CoreNet
{
    public interface IClashClient
    {

        Task<bool> ChangedConfig(ChangedClashConfig config);

        Task<Tuple<bool, string>> CheckUpdateCore(Action<string> check,bool uicheck);
        Task<List<Tuple<Config, string>>> GetAppclashYaml();
        Config GetClashCoreConfig();
        int GetClashPort();
        List<ProxyGroup> GetFileProxyGroups();
        List<IProxyItem> GetFileProxyLists();
        int GetPid();
        Task<bool> Init(string configpath = null);
        void SelectConfig(string path, RuleMode rulemode = RuleMode.Rule);
        void SelectProxies();
        Task StartCore();
        bool StopCore(int id);
        bool StopCore(string clashname);

        Task<ClashFileConfig> GetClashVersion();
    }
}