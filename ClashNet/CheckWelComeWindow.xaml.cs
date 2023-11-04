using ClashNet.ViewModels;
using SimpleUI.Controls;

namespace ClashNet
{
    /// <summary>
    /// CheckWelComeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckWelComeWindow : WindowBase
    {
        public CheckWelComeWindow(CheckWCViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
