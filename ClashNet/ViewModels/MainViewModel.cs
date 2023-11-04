using Clash.CoreNet;
using Clash.CoreNet.Helper;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet.Models.Interfaces;
using ClashNet.Models;
using ClashNet.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using SimpleUI.Interface.AppInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ZTest.Tools.Interfaces;

namespace ClashNet.ViewModels;

public partial class MainViewModel : ObservableRecipient, 
    IRecipient<AppBackground>,
    IRecipient<AppSystemProxyChanged>,
    IRecipient<AppAllowLanProxyChanged>

{
    DispatcherTimer timer = null;

    CancellationTokenSource tokensource = new();
    public MainViewModel(IAppNavigationViewService appNavigationView,
        ILocalSetting localSetting,
        IToastLitterMessage toastLitterMessage,
        IClashWebHttp clashWebHttp
        ,Serilog.ILogger logger)
    {
        AppNavigationView = appNavigationView;
        LocalSetting = localSetting;
        ClashClient = App.GetClashClient();
        ToastLitterMessage = toastLitterMessage;
        ClashWebHttp = clashWebHttp;
        Logger = logger;
        RunBrush =new(Colors.Red);
        IsActive = true;
        SystemList = new List<string>()
        {
            "开启系统代理",
            "关闭系统代理"
        };
    }

    [RelayCommand]
    async void Loaded()
    {
        var result = (await LocalSetting.ReadObjectConfig<AppBackground>("AppBackground") ?? new AppBackground());
        this.Appbackground = result;
        timer = new();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += Timer_Tick; ;
        timer.Start();
        await ClashWebHttp.InitMsg();
        await GetCoreSpend(tokensource.Token);
        Logger.Information("启动完毕");
    }

    [ObservableProperty]
    List<string> _SystemList;

    [ObservableProperty]
    long up;

    [ObservableProperty]
    string _Systemproxy;

    partial void OnSystemproxyChanged(string value)
    {
        string tag = (string)value;
        bool? val=false;
        switch (tag)
        {
            case "开启系统代理":
                val = true;
                break;
            case "关闭系统代理":
                val = false;
                break;
        }
        WeakReferenceMessenger.Default.Send(new AppSystemProxyChanged(val,null,null,null));
    }

    [ObservableProperty]
    long down;

    [ObservableProperty]
    bool _AllowLan;

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        var line = await Clash.CoreNet.ClashClient.GetCoreString(Api.CoreRun);
        if (line == null)
        {
            RunBrush = new(Colors.Red);
            return;
        }
        var jobject = JsonObject.Parse(line);
        if (jobject == null)
        {
            RunBrush = new(Colors.Red);
            return;
        }
        var hello = jobject["hello"]!.ToString();
        RunBrush = new(Colors.Green);
    }

    [ObservableProperty]
    SolidColorBrush _runBrush;
    public async Task GetCoreSpend(CancellationToken token)
    {
        try
        {
            HttpClient client = await HttpClientHelper.GetMsgHttpClient();
            Stream stream = await client.GetStreamAsync($"{Api.CoreRun}/traffic", token);
            StreamReader reader = new StreamReader(stream);
            while (true)
            {
               
                string? line = "";
                try
                {
                    line = await reader.ReadLineAsync()!;
                }
                catch (Exception)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    //这里捕获了错误就退出过程，线程等待3秒后，然后重新开始计数
                    await GetCoreSpend(tokensource.Token);
                    return;
                }
                var obj = JsonSerializer.Deserialize<CoreSpendArgs>(line!);
                this.Up = obj.Up;
                this.Down = obj.Down;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    [RelayCommand]
    void OpenAppLog()
    {
        Process.Start("explorer", Utils.LogPath);
    }

    [RelayCommand]
    void ChangedSystemProxy(bool flage)
    {
        if (flage)
        {
            string port = ClashClient.GetClashPort().ToString();
            ClashClient.RefershClient();
            var result = SystemProxyHelper.EnableProxy(Api.IP, port);
            if (result) ToastLitterMessage.Show($"已开启系统代理：{Api.IP}:{port}");
            else ToastLitterMessage.Show($"请手动输入IP：{Api.IP}    端口：{port}");
            Logger.Information($"已开启系统代理：{Api.IP}:{port}");
        }
        else
        {
            SystemProxyHelper.DisposeProxy();
            ToastLitterMessage.Show("已关闭系统代理");
            Logger.Information("关闭系统代理");
        }
    }

    [RelayCommand]
    async void RebootCore()
    {
        await Task.Run(async () =>
        {
            ClashClient.RefershClient();
            ClashClient.StopCore(ClashClient.GetPid());
            await ClashClient.StartCore().ConfigureAwait(false);
            Logger.Information("重启核心完毕");
        });
    }


    [RelayCommand]
    async void ChangdMode(string name)
    {
        ClashClient.RefershClient();
        switch (name)
        {
            case "全局":
                ClashC.Rulemode = Clash.CoreNet.Models.Enums.RuleMode.Global;
                await ClashClient.ChangedConfig(new Clash.CoreNet.Models.Clash.ChangedClashConfig() { mode= Clash.CoreNet.Models.Enums.RuleMode.Global.ToString() });
                break;
            case "规则":
                ClashC.Rulemode = Clash.CoreNet.Models.Enums.RuleMode.Rule;
                await ClashClient.ChangedConfig(new Clash.CoreNet.Models.Clash.ChangedClashConfig() { mode = Clash.CoreNet.Models.Enums.RuleMode.Rule.ToString() });
                break;
            case "直连":
                ClashC.Rulemode = Clash.CoreNet.Models.Enums.RuleMode.Direct;
                await ClashClient.ChangedConfig(new Clash.CoreNet.Models.Clash.ChangedClashConfig() { mode = Clash.CoreNet.Models.Enums.RuleMode.Direct.ToString() });
                break;
        }

        Logger.Information($"更换代理模式：{name}");
    }


    [ObservableProperty]
    string _title = "ClashNet";

    [RelayCommand]
    void SelectionChanged(Type type)
    {
        var content =   App.GetServices(type);
        AppNavigationView.Navigation(content, null);

        Logger.Information($"导航到{type.ToString()}");
    }

    [ObservableProperty]
    AppBackground _appbackground;

    public void Receive(AppBackground message)
    {
        this.Appbackground = message;
    }

    partial void OnAllowLanChanged(bool value)
    {
        ClashClient.RefershClient();
        ClashClient.ChangedConfig(new()
        {
            AllowLan = value
        });
        WeakReferenceMessenger.Default.Send(new AppAllowLanProxyChanged(value,$"局域网：{value}"));
    }

    public void Receive(AppSystemProxyChanged message)
    {
        switch (message.OpenSystemProxy)
        {
            case true:
                Systemproxy = "开启系统代理";
                break;
            case false:
                Systemproxy = "关闭系统代理";
                break;
        }
    }

    public void Receive(AppAllowLanProxyChanged message)
    {
        AllowLan = message.IsLan;
    }

    public IAppNavigationViewService AppNavigationView { get; }
    public ILocalSetting LocalSetting { get; }
    public IClashClient ClashClient { get; set; }
    public IToastLitterMessage ToastLitterMessage { get; }
    public IClashWebHttp ClashWebHttp { get; }
    public Serilog.ILogger Logger { get; }
}
