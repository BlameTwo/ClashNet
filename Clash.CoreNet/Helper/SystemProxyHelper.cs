using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Clash.CoreNet.Helper;

/// <summary>
/// 多平台开启系统代理
/// </summary>
public static class SystemProxyHelper
{
    static OSPlatform OSP { get;  } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true ? OSPlatform.Windows :
                                           RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == true ? OSPlatform.Linux : OSPlatform.OSX;
    
    /// <summary>
    /// 开启代理
    /// </summary>
    /// <param name="proxyServer">地址</param>
    /// <param name="proxyPort">端口</param>
    public static bool EnableProxy(string proxyServer, string proxyPort)
    {
        if(OSP == OSPlatform.Windows)
        {
            return WindowSystemProxy(proxyServer,proxyPort);
        }
        else if(OSP == OSPlatform.Linux)
        {
            return LinuxSystemProxy(proxyServer, proxyPort);
        }
        return false;
    }

    private static bool LinuxSystemProxy(string proxyServer, string proxyPort)
    {
        try
        {
            string proxy = $"{proxyServer}:{proxyPort}";

            Process.Start("gsettings", "set org.gnome.system.proxy mode 'manual'");

            Process.Start("gsettings", $"set org.gnome.system.proxy.http host '{proxy.Split(':')[0]}'");
            Process.Start("gsettings", $"set org.gnome.system.proxy.http port {proxy.Split(':')[1]}");

            Process.Start("gsettings", $"set org.gnome.system.proxy.https host '{proxy.Split(':')[0]}'");
            Process.Start("gsettings", $"set org.gnome.system.proxy.https port {proxy.Split(':')[1]}");

            Process.Start("gsettings", $"set org.gnome.system.proxy.ftp host '{proxy.Split(':')[0]}'");
            Process.Start("gsettings", $"set org.gnome.system.proxy.ftp port {proxy.Split(':')[1]}");

            Process.Start("gsettings", $"set org.gnome.system.proxy.socks host '{proxy.Split(':')[0]}'");
            Process.Start("gsettings", $"set org.gnome.system.proxy.socks port {proxy.Split(':')[1]}");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 关闭系统代理
    /// </summary>
    public static void DisposeProxy()
    {
        if (OSP == OSPlatform.Windows)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            key.SetValue("ProxyEnable", 0);
            key.Close();
        }
        else if (OSP == OSPlatform.Linux)
        {
            Process.Start("gsettings", "set org.gnome.system.proxy mode 'none'");
        }
    }

    static bool WindowSystemProxy(string proxyServer, string proxyPort)
    {
        try
        {
            string proxyAddress = $"{proxyServer}:{proxyPort}";
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            key.SetValue("ProxyEnable", 1);
            key.SetValue("ProxyServer", proxyAddress);
            key.Close();
            Process.Start("reg", "add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\" /v ProxyEnable /t REG_DWORD /d 1 /f");
            Process.Start("reg", "add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\" /v ProxyServer /d \"" + proxyAddress + "\" /f");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
