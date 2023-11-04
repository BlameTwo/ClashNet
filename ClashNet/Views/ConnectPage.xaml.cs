using ClashNet.ViewModels;
using System.Windows.Controls;

namespace ClashNet.Views;

/// <summary>
/// ConnectPage.xaml 的交互逻辑
/// </summary>
public partial class ConnectPage : Page
{
    public ConnectPage(ConnectViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
    }
}
