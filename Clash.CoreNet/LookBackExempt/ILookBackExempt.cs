using Clash.CoreNet.Models.Enums;

namespace Clash.CoreNet.LookBackExempt;

/// <summary>
/// 本地网络回环接口
/// </summary>
public interface ILookBackExempt
{
    /// <summary>
    /// 获得全部的UWP回环SID
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllUWPList();


    public Task<bool> SetLookBackExemptAsync(LookBackEnum lookBack,string sid);


}
