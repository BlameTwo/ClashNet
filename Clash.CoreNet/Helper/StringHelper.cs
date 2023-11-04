using System.Text.RegularExpressions;

namespace Clash.CoreNet.Helper;

public static class StringHelper
{
    /// <summary>
    /// 清除Base64字符串中的无用字符
    /// </summary>
    /// <param name="strIn"></param>
    /// <returns></returns>
    public static string CleanInput(this string strIn)
    {
        try
        {
            return Regex.Replace(strIn, @"[^a-zA-Z0-9\+/=]", "",
                                 RegexOptions.None, TimeSpan.FromSeconds(1.5));
        }
        catch (RegexMatchTimeoutException)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 修复Base64字符串
    /// </summary>
    /// <param name="strIn"></param>
    /// <returns></returns>
    public static string Base64Error(this string strIn)
    {
        string encodedString = strIn;
        int length = encodedString.Length;
        int remainder = length % 4;
        if (remainder == 2)
        {
            encodedString += "==";
        }
        else if (remainder == 3)
        {
            encodedString += "=";
        }
        return encodedString;
    }
}
