using Clash.CoreNet;
using Clash.CoreNet.Helper;
using Clash.CoreNet.Models.Clash;
using ClashNet.Models.VMMessage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ClashNet.ViewModels.DialogWinViewModels;

public partial class AddServerProViewModel:ObservableRecipient
{
    public AddServerProViewModel(IClashClient clashClient,ILogger logger)
    {
        ClashClient = clashClient;
        Logger = logger;
        Isaction = true;
    }

    [ObservableProperty]
    string url;

    [ObservableProperty]
    string ua;

    [ObservableProperty]
    string error;

    [ObservableProperty]
    string name;
    
    public IClashClient ClashClient { get; }
    public ILogger Logger { get; }

    [ObservableProperty]
    bool isaction;



    [ObservableProperty]
    bool? _dialogresult;


    partial void OnIsactionChanged(bool value)
    {
        GoFileCommand.NotifyCanExecuteChanged();
    }

    bool GetAction() => Isaction;

    [RelayCommand(CanExecute =nameof(GetAction))]
    async void GoFile()
    {
        Isaction = false;
        if (string.IsNullOrWhiteSpace(Url) || string.IsNullOrWhiteSpace(Name))
        {
            Error = "URL不能为空或名称不能为空";
            await Task.Delay(2000);
            Error = "";
            Logger.Information($"{this.GetType().Name}：URL为空");
            return; 
        }
        try
        {
            var result = await ProxyFormatHelper.UpdateFormUrl(Url);
            Debug.WriteLine(result.Count);
            var file = await ClashYamlHelper.MakeConfig(new MakeConfigData(result,Name,Ua, Clash.CoreNet.Models.Enums.RuleMode.Direct),Url);
            if (File.Exists(file.Item2))
            {
                WeakReferenceMessenger.Default.Send<RefershConfigFile>(new(true, "刷新", "刷新配置文件"));
                Logger.Information($"{this.GetType().Name}：下载配置文件成功");
                Dialogresult = true;
                Isaction = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
       
    }
}
