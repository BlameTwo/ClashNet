using ClashNet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ClashNet.ViewModels;

public partial class AboutViewModel:ObservableRecipient
{
    public AboutViewModel()
    {
        AboutList = new()
        {
            new("Clash Core","Clash core","Clash Core是一个基于规则的网络隧道。它可以捕获设备上任何应用程序的所有HTTP/HTTPS/TCP流量，并根据您定义的规则重定向到代理服务器。","https://github.com/Dreamacro/clash"),
            new("Community Mvvm Toolkit","Mvvm Tookit","Mvvm是一种开发模式，指代能够有效的解耦合前后端，起到多人开发，容易维护的作用。","https://github.com/CommunityToolkit/dotnet"),
            new("serilog","日志框架","帮助用户反馈错误以及Debug方便","https://serilog.net/"),
            new("XamlBehaviorsWpf","WPF行为","Behaviors指帮助对Mvvm包的一种界面解耦合的实现","https://github.com/Microsoft/XamlBehaviorsWpf"),
            new("VirtualizingWrapPanel","虚拟化面板","可控弹性布局的虚拟子项面板","https://github.com/sbaeumlisberger/VirtualizingWrapPanel"),
            new("H.NotifyIcon","托盘微标","使用Windows API实现的托盘库","https://github.com/HavenDV/H.NotifyIcon"),
        };
    }


    [ObservableProperty]
    List<AppAbout> _AboutList;
}
