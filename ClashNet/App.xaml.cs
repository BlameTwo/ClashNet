using Clash.CoreNet;
using Clash.CoreNet.Helper;
using ClashNet.Views.Windows;
using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleUI.Interface;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using ZTest.Tools.Interfaces;

namespace ClashNet;

public partial class App : Application
{
    public IHost Host { get; set; }

    public App()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .RegisterViews()
            .RegisterUI()
            .RegisterClash()
            .Build();
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        this.Exit += App_Exit;
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            App.GetServices<ILogger>().Information(e.Exception.Message);
        }
        catch (Exception)
        {
            Debug.WriteLine(e.Exception.StackTrace);
        }
        e.Handled = true;
    }

    private void App_Exit(object sender, ExitEventArgs e)
    {
        var client = App.GetClashClient();
        client.StopCore(client.GetPid());
    }

    ILogger log=null;

    protected async override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        FileHelper.FolderExits(AppProperty.AppConfigFolder);
        FileHelper.FolderExits(AppProperty.AppClashCacheFolder);
        FileHelper.FolderExits(AppProperty.ClashConfigFolder);
        FileHelper.FolderExits(AppProperty.CacheFolder);
        var theme = App.GetServices<IThemeApply<App>>();
        var localsetting = GetServices<ILocalSetting>();
        log = GetServices<ILogger>();
        TranWindow = GetServices<ClashNet.Services.IWindowManager>();
        CheckClash();
        CreateIcon();
        log.Information("应用启动");
        await localsetting.InitSetting();
        theme.App = this;
        theme.IsEnable = true;
        var themesetting = (await localsetting.ReadConfig("Theme"))?.ToString() ?? "";
        if (!string.IsNullOrWhiteSpace(themesetting))
        {
            Utils.ChangTheme(themesetting, theme);
            log.Information($"更改应用颜色：{themesetting}");
        }
        MainWindow window = App.GetServices<MainWindow>();
        this.MainWindow = window;
        TranWindow.InitWindow(Models.WindowEnum.Main, window);
        TranWindow.Show(Models.WindowEnum.Main);
        TranWindow.InitWindow(Models.WindowEnum.Lookback, App.GetServices<WindowLookBack>());
    }


    TaskbarIcon icon = new();
    RelayCommand TrayLeftClick { get; set; }
    Services.IWindowManager TranWindow { get; set; }

    private void CheckClash()
    {
        var dialog = App.GetServices<CheckWelComeWindow>();
        if (dialog.ShowDialog() != true)
        {
            Environment.Exit(0);
            log.Information("Dialog 退出，应用关闭。");
        }
        TranWindow.InitWindow(Models.WindowEnum.Tran,GetServices<TranIconWindow>());
    }

    /// <summary>
    /// 创建通知栏图标
    /// </summary>
    private void CreateIcon()
    {
        this.TrayLeftClick = new(() =>
        {
            if (TranWindow.TranShow)
            {
                TranWindow.Hide(Models.WindowEnum.Tran);
            }
            else
            {
                TranWindow.Show(Models.WindowEnum.Tran);
            }
        });
        icon.ForceCreate(false);
        icon.ToolTipText = "Clash";
        icon.IconSource = new BitmapImage(new("pack://application:,,,/ClashNet;component/Resources/Images/icon.ico"));
        icon.LeftClickCommand = this.TrayLeftClick;
        log.Information("创建托盘图标……");
    }
}
