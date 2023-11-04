using YamlDotNet.Serialization;

namespace Clash.CoreNet.Models;

/// <summary>
/// 规则
/// </summary>
public class RuleFormatData
{
    [YamlMember(Alias = "payload")]
    public List<string> Rules { get; set; }
}
