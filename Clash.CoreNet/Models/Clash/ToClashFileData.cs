using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;

namespace Clash.CoreNet.Models.Clash;

public record ToClashFileData(RuleMode rulemod,IProxyItem item);

public record MakeConfigData(List<IProxyItem> items, string name, string UA,RuleMode RuleMode = RuleMode.Direct);