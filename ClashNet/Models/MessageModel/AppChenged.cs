namespace ClashNet.Models;


public record AppSystemProxyChanged(bool? OpenSystemProxy,string ProxyName,string ProxyPort,string Message);

public record AppAllowLanProxyChanged(bool IsLan, string Message);

public record AppLookbackChanged(bool IsOpen,string message);
