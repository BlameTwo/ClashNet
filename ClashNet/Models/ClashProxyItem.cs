using Clash.CoreNet;
using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet.Models.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ClashNet.Models;

[INotifyPropertyChanged]
public partial class ClashProxyItem : IProxyItem
{
    public string Name { get; set; }
    public string Port { get; set; }
    public string Server { get; set; }
    public ProxyItemEnum Type { get; set; }

    [ObservableProperty]
    object _delay;


    public object Clone()
    {
        return this.MemberwiseClone();
    }

    /// <summary>
    /// 刷新延迟
    /// </summary>
    public async void RefershDelay()
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        string encodedName = this.Name.Replace("+", "%20");

        string url = $"{Api.CoreRun}/proxies/{encodedName}/delay?timeout={AppProperty.ProxyItemPing}&url={AppProperty.ProxyItemTestUrl}";
        HttpClient client = new(handler) { Timeout = TimeSpan.FromSeconds(10) };

        var quest = await client.SendAsync(new HttpRequestMessage()
        {
            Method = System.Net.Http.HttpMethod.Get,
            RequestUri = new System.Uri(url)
        });
        var stream = await quest!.Content.ReadAsStreamAsync();
        var str = await quest!.Content.ReadAsStringAsync();
        var job = JsonObject.Parse(str);
        if (job["message"] != null) //如果测试出现了错误
        {
            var str2 = job["message"].GetValue<string>();
            this.Delay = str2;
            return;
        }
        ProxyDelay delay = await JsonSerializer.DeserializeAsync<ProxyDelay>(stream)!;
        this.Delay = delay.Delay;
    }

    [RelayCommand]
    public void DoubleClickDelay() => RefershDelay();
}
