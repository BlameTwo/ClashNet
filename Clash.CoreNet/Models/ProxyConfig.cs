namespace Clash.CoreNet.Models;

public class ProxyConfig
{
    public int Port { get; set; } = 7895;

    public int APIPort { get; set; } = 9090;

    public int HttpPort { get; set; } = 7890;

    public int MixedPort { get; set; } = 7888;

    public string LogLevel { get; set; } = "info";

    public string address { get; set; } = "*";

    public bool IPv6 { get; set; } = true;

    public int SocksPort { get; set; } = 7891;

    public bool OpenLan { get; set; } = true;
}
