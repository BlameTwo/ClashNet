using Clash.CoreNet;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet.Models.Interfaces;
using ClashNet.DialogWindows;
using ClashNet.Models;
using ClashNet.Services;
using ClashNet.ViewModels;
using ClashNet.ViewModels.DialogWinViewModels;
using ClashNet.ViewModels.Windows;
using ClashNet.Views;
using ClashNet.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleUI.Interface;
using SimpleUI.Interface.AppInterfaces;
using SimpleUI.Interface.AppInterfaces.Services;
using SimpleUI.Services;
using SimpleUI.Themes;
using SimpleUI.Utils;
using System;
using ZTest.Tools.Interfaces;
using ZTest.Tools.Services;

namespace ClashNet;

public static class RegisterHost
{
    /// <summary>
    /// 创建视图和视图模型
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostBuilder RegisterViews(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<CheckWelComeWindow>();
            services.AddSingleton<CheckWCViewModel>();

            services.AddTransient<ConfigPage>();
            services.AddTransient<ConfigViewModel>();

            services.AddTransient<ProxyPage>();
            services.AddTransient<ProxyViewModel>();

            services.AddTransient<LogPage>();
            services.AddTransient<LogViewModel>();

            services.AddTransient<SettingPage>();
            services.AddTransient<SettingViewModel>();

            services.AddTransient<ConnectPage>();
            services.AddTransient<ConnectViewModel>();


            services.AddTransient<AboutPage>();
            services.AddTransient<AboutViewModel>();


            #region 注册对话框窗口
            services.AddTransient<AddServerProfile>();
            services.AddTransient<AddServerProViewModel>();
            #endregion

            #region 本地回环
            services.AddSingleton<WindowLookBack>();
            services.AddSingleton<LookBackViewModel>();
            #endregion

            #region 注册托盘窗口
            services.AddSingleton<TranIconWindow>();
            services.AddSingleton<TranIconViewModel>();
            services.AddSingleton<ClashNet.Services.IWindowManager,WindowManager>();

            #endregion
            var log =  new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.File("logs/app.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit:31,
                retainedFileTimeLimit:TimeSpan.FromDays(2)
                ,rollOnFileSizeLimit:true,
                fileSizeLimitBytes:52428800)
                .CreateLogger();
            services.AddSingleton<ILogger>(log);
        });
        return builder;
    }

    public static IHostBuilder RegisterUI(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IToastLitterMessage, ToastLitterMessage>();
            services.AddSingleton<IShowDialogService, ShowDialogService>();
            services.AddSingleton<IThemeApply<App>, ThemeApply<App>>();
            services.AddSingleton<ILocalSetting, LocalSetting>();
            services.AddSingleton<IAppNavigationViewService, AppNavigationViewService>();
        });
        return builder;
    }

    /// <summary>
    /// 刷新最近操作的Client
    /// </summary>
    /// <param name="client"></param>
    public static void RefershClient(this IClashClient client) =>App.GetClashClient();

    public static IHostBuilder RegisterClash(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ///注册clash客户端
            services.AddSingleton<IClashClient,ClashClient>();
            services.AddSingleton<IClashWebHttp,ClashWebHttp>();
        });
        return builder;
    }
}

public partial class App
{
    public static T GetServices<T>()
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new System.Exception($"注册项目缺少{typeof(T)}");
        }
        return service;
    }
    public static IClashClient GetClashClient() => (Current as App)!.Host.Services.GetRequiredService<IClashClient>();

    public static object GetServices(Type type)
    {
        if ((App.Current as App)!.Host.Services.GetService(type) == null)
        {
            throw new System.Exception($"注册项目缺少{type}");
        }
        return (App.Current as App)!.Host.Services.GetService(type)!;
    }
}
