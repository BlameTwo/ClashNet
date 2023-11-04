using Clash.CoreNet;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using System;
using System.Threading.Tasks;
using ZTest.Tools.Interfaces;

namespace ClashNet.ViewModels;

public partial class CheckWCViewModel:ObservableRecipient
{
    public CheckWCViewModel(IClashClient clashClient,ILocalSetting localSetting,ILogger logger)
    {
        ClashClient = clashClient;
        LocalSetting = localSetting;
        Logger = logger;
    }

    [ObservableProperty]
    bool? _DialogResult;

    [ObservableProperty]
    string _tipmessage;

    public IClashClient ClashClient { get; }
    public ILocalSetting LocalSetting { get; }
    public ILogger Logger { get; }

    [RelayCommand]
    async void Loaded()
    {
        Logger.Information("开始检查更新");
        var flage2 = System.Convert.ToBoolean((await LocalSetting.ReadConfig("SkipCheck"))?.ToString() ?? "false");
        if (flage2)
        {
            DialogResult = true;

            Logger.Information($"{this.GetType().Name}：设置跳过检查");
            return;
        }
        Tipmessage = "[1/4]正在刷新Client，并验证Core完整性";
        ClashClient.RefershClient();
        var result = await ClashClient.CheckUpdateCore((s) =>
        {
            Tipmessage = $"[1/4]{s}";
            Logger.Information(s);
        },true);
        if (!result.Item1)
        {
            Tipmessage = $"[1/4]更新失败：{result.Item2}";
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            Logger.Information(result.Item2);
        }
        string lastconfig = (await LocalSetting.ReadConfig("LastConfig"))?.ToString() ?? "";
        await Task.Delay(TimeSpan.FromSeconds(0.4));
        Tipmessage="[2/4]正在启动Core";
        var flage =  await ClashClient.Init(lastconfig);
        await Task.Delay(TimeSpan.FromSeconds(0.8));
        Tipmessage ="[3/4]正在检查Core可用性";
        while (true)
        {
            var line = await Clash.CoreNet.ClashClient.GetCoreString(Api.CoreRun);
            if (line == null)
            {
                Tipmessage = "[3/4]Core启动失败，正在重试";
                await Task.Delay(TimeSpan.FromSeconds(1));
                Logger.Information("连接Core错误，重试……");
                ClashClient.StopCore(ClashClient.GetPid());
                await ClashClient.StartCore();
            }
            if (line != null)
                break;
        }
        Tipmessage = "[4/4]启动主窗体，请稍作等待";
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        DialogResult = true;
    }

    [RelayCommand]
    void Skip()
    {
        DialogResult = true;

        Logger.Information($"{this.GetType().Name}：手动跳过检查");
    }
}
