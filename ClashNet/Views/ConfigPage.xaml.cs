using ClashNet.ViewModels;
using SimpleUI.Interface;
using System.Windows.Controls;

namespace ClashNet.Views;

/// <summary>
/// ConfigPage.xaml 的交互逻辑
/// </summary>
public partial class ConfigPage : Page,IPage
{
    public ConfigPage(ConfigViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
    }

    public void NavigationLeaved(object data)
    {
    }

    public void NavigationLoaded(object data)
    {
    }

}
