using ClashNet.ViewModels;
using System.Windows.Controls;

namespace ClashNet.Views;

/// <summary>
/// SettingPage.xaml 的交互逻辑
/// </summary>
public partial class SettingPage : Page
{
    public SettingPage(SettingViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
    }
}
