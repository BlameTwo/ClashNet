using Clash.CoreNet;
using ClashNet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using Serilog;
using SimpleUI.Interface;
using SimpleUI.Interface.AppInterfaces;
using System;
using ZTest.Tools.Interfaces;

namespace ClashNet.ViewModels;

public partial class SettingViewModel:ObservableRecipient
{
    public SettingViewModel(ILocalSetting localSetting,IToastLitterMessage toastLitterMessage,IThemeApply<App> themeApply,IClashClient clashClient,ILogger logger)
    {
        LocalSetting = localSetting;
        ToastLitterMessage = toastLitterMessage;
        ThemeApply = themeApply;
        ClashClient = clashClient;
        Logger = logger;
        IsActive = true;
    }

    [RelayCommand]
    async void Loaded()
    {
        this.Themestr = (await LocalSetting.ReadConfig("Theme"))?.ToString() ?? "暗色";
        this.Skipcheck = System.Convert.ToBoolean((await LocalSetting.ReadConfig("SkipCheck"))?.ToString() ?? "false");
        var result = (await LocalSetting.ReadObjectConfig<AppBackground>("AppBackground")??new AppBackground());
        this.Opacity = result.Opacity;
        this.Imagepath = result.ImagePath;
        this.Gaussian = result.Gaussian;
        ClashClient.RefershClient();
        var config = await ClashClient.GetClashVersion();
        this.Clashversion = config == null ? "文件不存在，请检查更新" : config.Version;
        Logger.Information($"{this.GetType().Name}：Core不存在或文件不存在");
        RefLogSize();
    }

    [ObservableProperty]
    string _AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

    [RelayCommand]
    void RefLogSize()
    {
        
        string[] files = System.IO.Directory.GetFiles(Utils.LogPath);
        long totalSize = 0;
        foreach (string file in files)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            totalSize += fileInfo.Length;
        }
        LogFileSize = Math.Round((totalSize / 1024.0 / 1024.0),2).ToString()+"MB";
    }

    [ObservableProperty]
    string _LogFileSize = "";

    [ObservableProperty]
    string themestr;

    [ObservableProperty]
    double opacity;

    [ObservableProperty]
    double gaussian;

    [ObservableProperty]
    bool skipcheck;

    [ObservableProperty]
    string imagepath;

    [ObservableProperty]
    string clashversion;


    public async void SaveConfig<T>(string key,T value)
    {
        await LocalSetting.SaveConfig(key, value);
    }

    partial void OnGaussianChanged(double value)
    {
        ChangedBack();
    }

    partial void OnImagepathChanged(string value)
    {
        ChangedBack();
    }

    [RelayCommand]
    async void UpDateCore()
    {
        ClashClient.RefershClient();
        if ((await ClashClient.CheckUpdateCore((s) =>
        {
            ToastLitterMessage.Show(s);
        },false)).Item1)
        {
            ToastLitterMessage.Show("更新完毕……");
            Logger.Information($"{this.GetType().Name}：Core更新完毕");
        }
    }


    partial void OnOpacityChanged(double value)
    {
        ChangedBack();
    }

    [RelayCommand]
    void OpenGithub()
    {
        ToastLitterMessage.Show("哪有什么发布页，都是假莉！");
    }
    void ChangedBack()
    {
        var back = new AppBackground()
        {
            ImagePath = Imagepath,
            Gaussian = Gaussian,
            Opacity = this.Opacity
        };
        WeakReferenceMessenger.Default.Send<AppBackground>(back);
        SaveConfig("AppBackground", back);

        Logger.Information($"{this.GetType().Name}：更改背景透明度和高斯模糊");
    }

    [RelayCommand]
    void OpenFile()
    {
        OpenFileDialog dialog = new();
        dialog.Filter = "图片文件(*.jpg,*.bmp,*.png)|*.jpg;*.bmp;*.png";
        if (dialog.ShowDialog() != true) return;
        this.Imagepath = dialog.FileName;
        Logger.Information($"{this.GetType().Name}：更改背景图片：{dialog.FileName}");
    }

    partial void OnThemestrChanged(string value)
    {
        Utils.ChangTheme(value, this.ThemeApply);
        SaveConfig("Theme", value);
        Logger.Information($"{this.GetType().Name}：更改主题颜色为{value}");
    }


    partial void OnSkipcheckChanged(bool value)
    {
        SaveConfig("SkipCheck", value);
        Logger.Information($"{this.GetType().Name}：设置跳过检查：{value}");
    }

    public ILocalSetting LocalSetting { get; }
    public IToastLitterMessage ToastLitterMessage { get; }
    public IThemeApply<App> ThemeApply { get; }
    public IClashClient ClashClient { get; }
    public ILogger Logger { get; }
}
