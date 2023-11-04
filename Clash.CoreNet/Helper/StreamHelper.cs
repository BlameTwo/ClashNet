namespace Clash.CoreNet.Helper;

public static class StreamHelper
{
    /// <summary>
    /// 流=>字符串
    /// </summary>
    /// <param name="stream">流</param>
    /// <returns></returns>
    public static string StreamToString(this Stream stream)
    {
        stream.Position = 0;
        StreamReader reader = new StreamReader(stream);
        string text = reader.ReadToEnd();
        reader.Close();
        return text;
    }
}
