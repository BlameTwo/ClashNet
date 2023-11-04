using Clash.CoreNet.Helper;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ClashNet.ViewModels;

public partial class LogViewModel:ObservableRecipient
{
    public LogViewModel()
    {
        this.CoreLogs = new();
    }

    [ObservableProperty]
    string logdata="";

    [ObservableProperty]
    ObservableCollection<CoreLogData> _CoreLogs;


    [RelayCommand]
    async void Loaded()
    {
        await GetLogSpend();
    }

    public async Task GetLogSpend()
    {
        try
        {
            HttpClient client = await HttpClientHelper.GetMsgHttpClient();
            Stream stream = await client.GetStreamAsync($"{Api.CoreRun}/logs");
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
                    continue;
                }
                var obj = JsonSerializer.Deserialize<CoreLogData>(line!);
                this.CoreLogs.Add(obj);
            }
        }
        catch (Exception)
        {
        }
    }
}
