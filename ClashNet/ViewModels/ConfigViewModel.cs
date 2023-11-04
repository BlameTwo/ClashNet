using Clash.CoreNet;
using Clash.CoreNet.Helper;
using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Clash;
using Clash.CoreNet.Models.Interfaces;
using ClashNet.DialogWindows;
using ClashNet.Models;
using ClashNet.Models.VMMessage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using SimpleUI.Interface.AppInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ZTest.Tools.Interfaces;

namespace ClashNet.ViewModels;

public partial class ConfigViewModel : ObservableRecipient, IRecipient<RefershConfigFile>
{
    public ConfigViewModel(IClashClient clashClient, 
        ILocalSetting localSetting, 
        IToastLitterMessage toastLitterMessage,
        IClashWebHttp clashWebHttp,
        ILogger logger)
    {
        ClashClient = clashClient;
        LocalSetting = localSetting;
        ToastLitterMessage = toastLitterMessage;
        ClashWebHttp = clashWebHttp;
        Logger = logger;
        Yamlconfig = new();
        IsActive = true;
    }

    [RelayCommand]
    async void Loaded()
    {
        refersh();
        
    }

    async void refersh()
    {
        this.Yamlconfig.Clear();
        this.Yamlconfig = await ClashClient.GetAppclashYaml();
        ClashClient.RefershClient();
        var config = ClashClient.GetClashCoreConfig();
        if (config == null)
        {
            ToastLitterMessage.Show("当前选择为应用生成的临时配置文件：main");
        }
        foreach (var item in Yamlconfig)
        {
            if (config == null) break;
            if (item.Item1.Guid == config.Guid)
            {
                Selectitem = item;
                break;
            }
            else if (item.Item1.Name == "main")
            {
                Selectitem = item;
            }
        }
        if(Selectitem != null)
            await LocalSetting.SaveConfig<string>("LastConfig", Selectitem.Item2);
        Logger.Information($"{this.GetType().Name}刷新配置完毕");
    }

    [RelayCommand]
    void RefershFile() => refersh();

    [RelayCommand]
    async void FromCilpboard()
    {
        var text = Clipboard.GetText();
        if (string.IsNullOrWhiteSpace(text)) return;
        List<IProxyItem> resultlist = new();
        try
        {
            resultlist = await ProxyFormatHelper.UpdateFormUrl(text);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); return; }
        Random ran = new(1000);
        var file = await ClashYamlHelper.MakeConfig(new MakeConfigData(resultlist, $"ImportClipboard{ran.Next().ToString()}", null, Clash.CoreNet.Models.Enums.RuleMode.Direct),text);
        ToastLitterMessage.Show($"增加了1个配置文件,{resultlist.Count}个代理IP");
        Logger.Information($"{this.GetType().Name}：增加了1个配置文件,{resultlist.Count}个代理IP");
        refersh();
    }

    [ObservableProperty]
    List<Tuple<Config, string>> yamlconfig;

    [RelayCommand]
    async void UpdateConfig()
    {
        var result = await ClashYamlHelper.UpdateConfig(Selectitem.Item2,Selectitem.Item1.Guid);
        ToastLitterMessage.Show(result.Item2);
        Logger.Information($"{this.GetType().Name}：更新配置文件完毕");
    }
    

    public IClashClient ClashClient { get; set; }
    public ILocalSetting LocalSetting { get; }
    public IToastLitterMessage ToastLitterMessage { get; }
    public IClashWebHttp ClashWebHttp { get; }
    public ILogger Logger { get; }

    [RelayCommand]
    void ShowAddServerProfile()
    {
        var asp = App.GetServices<AddServerProfile>();
        asp.ShowDialog();
    }

    [RelayCommand]
    void OpenConfigFile()
    {
        Process.Start("explorer", Selectitem.Item2);
        Logger.Information($"{this.GetType().Name}：打开文件:{Selectitem.Item2}");
    }

    [RelayCommand]
    void DeleteConfigFile()
    {
        File.Delete(Selectitem.Item2);
        refersh();
    }

    [RelayCommand]
    void SelectConfigFile()
    {
        if (Selectitem == null)
        {
            ToastLitterMessage.Show("请选择一个配置文件");
            return;
        }
        ClashClient.SelectConfig(Selectitem.Item2,ClashC.Rulemode);
        LocalSetting.SaveConfig<string>("LastConfig", Selectitem.Item2);
        ToastLitterMessage.Show("切换配置文件成功！");
        Logger.Information($"{this.GetType().Name}：切换配置文件为：{Selectitem.Item2}");
    }

    [ObservableProperty]
    Tuple<Config, string> selectitem;



    public void Receive(RefershConfigFile message)
    {
        if (message.isrefersh)
        {
            refersh();
        }
        Debug.WriteLine(message.log);
    }
}
