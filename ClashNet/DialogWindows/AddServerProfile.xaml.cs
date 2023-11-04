using ClashNet.ViewModels.DialogWinViewModels;
using SimpleUI.Controls;
using System.Windows;

namespace ClashNet.DialogWindows;

/// <summary>
/// AddServerProfile.xaml 的交互逻辑
/// </summary>
public partial class AddServerProfile : WindowBase
{
    public AddServerProfile(AddServerProViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
    }
}
