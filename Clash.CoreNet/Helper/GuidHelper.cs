namespace Clash.CoreNet.Helper;

internal static class GuidHelper
{
    /// <summary>
    /// 获取新的Guid
    /// </summary>
    /// <returns></returns>
    internal static Guid GetNewGuid() => Guid.NewGuid();

    /// <summary>
    /// 格式化Guid，只做对比用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static Guid ParseGuid(string input) => Guid.Parse(input);
}
