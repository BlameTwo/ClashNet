using CommunityToolkit.Mvvm.ComponentModel;
using Clash.CoreNet.Models.EventHandler;
using System.Windows.Documents;
using System.Collections.Generic;
using Clash.CoreNet.Models.Interfaces;
using CommunityToolkit.Mvvm.Input;
using SimpleUI.Interface.AppInterfaces;
using System.Collections.ObjectModel;
using System.Linq;
using ZTest.Tools;
namespace ClashNet.ViewModels;

public partial class ConnectViewModel:ObservableRecipient
{
    public ConnectViewModel(IClashWebHttp clashWebHttp,IToastLitterMessage toastLitterMessage)
    {
        IsActive = true;
        ClashWebHttp = clashWebHttp;
        ToastLitterMessage = toastLitterMessage;
    }

    [RelayCommand]
    async void Loaded()
    {
        RefList();
    }

    [RelayCommand]
    async void Delete()
    {
        if (this.SelectItem == null)
        {
            ToastLitterMessage.Show("请选择一个连接");
            return;
        }
        var result =  await ClashWebHttp.DelectConnection(SelectItem.ID);
        Connections.Remove(SelectItem);
    }

    [RelayCommand]
    async void DeleteAll()
    {
        if (this.Connections == null || this.Connections.Count == 0)
        {
            return;
        }
        foreach (var item in Connections.ToList())
        {
            var result = await ClashWebHttp.DelectConnection(item.ID);
            Connections.Remove(item);
        }
    }

    [RelayCommand]
    async void RefList()
    {
        var resultlist = await ClashWebHttp.GetConnections();
        if (Connections == null)
            Connections = new();
        else
            this.Connections.Clear();
        Connections = resultlist.Connections.ToObservableList();
    }

    [ObservableProperty]
    Connection _SelectItem;

    [ObservableProperty]
    ObservableCollection<Connection> _Connections;

    public IClashWebHttp ClashWebHttp { get; }
    public IToastLitterMessage ToastLitterMessage { get; }
}
