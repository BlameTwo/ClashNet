using Clash.CoreNet;
using Clash.CoreNet.Helper;
using Clash.CoreNet.Models.Interfaces;
using ClashNet.Models;
using ClashNet.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;

namespace ClashNet.ViewModels.Windows;

public partial class TranIconViewModel : 
    ObservableRecipient, 
    IRecipient<AppSystemProxyChanged>,
    IRecipient<AppAllowLanProxyChanged>
{
    public TranIconViewModel(IClashClient clashClient,IClashWebHttp clashWebHttp)
    {
        IsActive = true;
        ClashClient = clashClient;
        ClashWebHttp = clashWebHttp;
    }

    public IClashClient ClashClient { get; set; }
    public IClashWebHttp ClashWebHttp { get; }

    private IWindowManager _wm { get; set; }

    /// <summary>
    /// 显示方法
    /// </summary>
    public async void ShowMethod()
    {
        _wm = App.GetServices<IWindowManager>();
        ClashClient.RefershClient();
        if (ProxyList != null)
            ProxyList.Clear();
        ProxyList = ClashClient.GetFileProxyLists();
        var result = await ClashWebHttp.GetProxies();
        result.Items.ForEach((val) =>
        {
            if (val.Name == "GLOBAL")
            {
                foreach (var item in ProxyList)
                {
                    if(val.NowSelectName == item.Name)
                    {
                        this.SelectProxy = item;
                    }
                }
            }
        });
    }


    [ObservableProperty]
    List<IProxyItem> _ProxyList;


    [ObservableProperty]
    IProxyItem _SelectProxy;

    [ObservableProperty]
    bool _OpenSystemProxy;

    [ObservableProperty]
    bool _OpenAllowProxy;


    partial void OnOpenAllowProxyChanged(bool value)
    {
        ClashClient.RefershClient();
        ClashClient.ChangedConfig(new()
        {
            AllowLan = value
        });
        WeakReferenceMessenger.Default.Send(new AppAllowLanProxyChanged(value, $"局域网：{value.ToString().ToLower()}"));
    }

    partial void OnOpenSystemProxyChanged(bool value)
    {
        if (value)
        {
            string port = ClashClient.GetClashPort().ToString();
            ClashClient.RefershClient();
            SystemProxyHelper.EnableProxy(Api.IP, port);
        }
        else
        {
            SystemProxyHelper.DisposeProxy();
        }
        WeakReferenceMessenger.Default.Send(new AppSystemProxyChanged(value,null,null,null)); 
    }



    [RelayCommand]
    void ExitApp()
    {
        ClashClient.RefershClient();
        ClashClient.StopCore(ClashClient.GetPid());
        Environment.Exit(0);
    }

    [RelayCommand]
    void ShowHome()
    {
        if (_wm.MainShow)
        {
            _wm.Hide(Models.WindowEnum.Main);
        }
        else
        {
            _wm.Show(Models.WindowEnum.Main);
        }
    }

    [RelayCommand]
    void ShowLookback()
    {
        if (_wm.LookbackShow)
        {
            _wm.Hide(Models.WindowEnum.Lookback);
        }
        else
        {
            _wm.Show(Models.WindowEnum.Lookback);
        }
    }

    public void Receive(AppSystemProxyChanged message)
    {
        OpenSystemProxy = (bool)message.OpenSystemProxy!;
    }

    public void Receive(AppAllowLanProxyChanged message)
    {
        OpenAllowProxy = message.IsLan;
    }

}
