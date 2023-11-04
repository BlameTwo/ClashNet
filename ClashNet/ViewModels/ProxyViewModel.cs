using Clash.CoreNet;
using Clash.CoreNet.Models;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet.Models.Interfaces;
using ClashNet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SimpleUI.Controls;
using SimpleUI.Interface.AppInterfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZTest.Tools;

namespace ClashNet.ViewModels;

public partial class ProxyViewModel:ObservableRecipient
{
    public ProxyViewModel(IClashClient clashClient,IClashWebHttp clashWebHttp,IToastLitterMessage toastLitterMessage
        ,ILogger logger)
    {
        IsActive = true;
        ClashClient = clashClient;
        ClashWebHttp = clashWebHttp;
        ToastLitterMessage = toastLitterMessage;
        Logger = logger;
        this.Groups = new(); Proxyitems = new();
    }

    [RelayCommand]
    async void Loaded()
    {
        ClashClient.RefershClient();
        try
        {
            //获得全部的代理
            _configallproxy = ClashClient.GetFileProxyLists();
            //获得代理分组
            var grouplist = ClashClient.GetFileProxyGroups();
            this.Groups = grouplist;
        }
        catch (System.Exception)
        {
            ToastLitterMessage.Show("请前往配置页面选择一个配置文件");
        }
    }

    List<IProxyItem> _configallproxy;



    [ObservableProperty]
    ProxyGroup selectgroup;

    partial void OnSelectgroupChanged(ProxyGroup value)
    {

        Proxyitems.Clear();
        foreach (var item in value.Proxies)
        {
            foreach (var item2 in _configallproxy)
            {
                if(item == item2.Name)
                {
                    var result = item2 as ClashProxyItem;
                    Proxyitems.Add(new()
                    {
                        Name = item2.Name,
                         Port=item2.Port,
                        Server = item2.Server,
                         Type = item2.Type
                    });
                    RefershSelectItem();
                }
            }
        }
    }

    /// <summary>
    /// 刷新选择的代理节点
    /// </summary>
    async void RefershSelectItem()
    {
        var result = await ClashWebHttp.GetProxies();
        foreach (var item in result.Items)
        {
            if(this.Selectgroup.Name != item.Name)
                continue;
            foreach (var item2 in Proxyitems)
            {
                if (item2.Name != item.NowSelectName) continue;
                Selectproxyitem = item2;
            }
        }
    }


    [RelayCommand]
    async void SelectChangedGlobProxy()
    {
        try
        {
            if (this.Selectgroup.Type != "select")
            {
                ToastLitterMessage.Show("此更换节点仅支持Selecter类型策略(自动选择不支持)");
                return;
            }
            //手动选择的
            var result = await ClashWebHttp.ChangSelectProxy(this.Selectgroup.Name, this.Selectproxyitem.Name);
            //另外一个系统定义的手动选择
            var result2 = await ClashWebHttp.ChangSelectProxy("GLOBAL", this.Selectproxyitem.Name);
            ToastLitterMessage.Show("切换成功！");
            Logger.Information($"{this.GetType().Name}：更该全局代理：{this.Selectproxyitem.Name}");
        }
        catch (System.Exception)
        {
            ToastLitterMessage.Show("切换失败");
        }
    }

    [ObservableProperty]
    ObservableCollection<CoreProxiesItem> _proxies;

    [ObservableProperty]
    ObservableCollection<ClashProxyItem> _proxyitems;

    [ObservableProperty]
    ClashProxyItem selectproxyitem;

    [ObservableProperty]
    List<ProxyGroup> _groups;

    public IClashClient ClashClient { get; set; }
    public IClashWebHttp ClashWebHttp { get; }
    public IToastLitterMessage ToastLitterMessage { get; }
    public ILogger Logger { get; }
}
