using Clash.CoreNet.LookBackExempt.Toolkit;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ZTest.Tools;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using Clash.CoreNet.LookBackExempt;

namespace ClashNet.ViewModels.Windows;

public partial class LookBackViewModel:ObservableRecipient
{
    LoopUtil LoopUtil = new();

    public LookBackViewModel()
    {
        LoopUtil.LoadApps();
        this.Apps = LoopUtil.Apps.ToObservableList() ;
    }

    [RelayCommand]
    void SelectAll()
    {
        for (int i = 0; i < Apps.Count; i++)
        {
            Apps[i].LoopUtil = true;
        }
    }

    [RelayCommand]
    void BackSelectAll()
    {
        for (int i = 0; i < Apps.Count; i++)
        {
            Apps[i].LoopUtil = !Apps[i].LoopUtil;
        }
    }

    [RelayCommand]
    void Save()
    {
        LoopUtil.SaveLoopbackState(this.Apps.ToList());
    }

    [RelayCommand]
    void Refersh()
    {
        LoopUtil.LoadApps();
        this.Apps.Clear();
        this.Apps = LoopUtil.Apps.ToObservableList();
    }

    [ObservableProperty]
    ObservableCollection<AppContainer> _Apps;

}
