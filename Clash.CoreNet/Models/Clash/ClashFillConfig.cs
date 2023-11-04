namespace Clash.CoreNet.Models.Clash;

/// <summary>
/// Clash 下载之后保存在配置文件中的一个json文件，以确定下次更新是否可以更新
/// </summary>
public class ClashFileConfig
{
    public string Version { get; set; }

    public DateTime CreateTime { get; set; }

    public string TagName { get; set; }

    public string FileName { get; set; }
}
