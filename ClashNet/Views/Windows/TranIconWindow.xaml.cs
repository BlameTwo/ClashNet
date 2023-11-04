using ClashNet.Services;
using ClashNet.ViewModels.Windows;
using SimpleUI.Controls;

namespace ClashNet.Views.Windows
{
    /// <summary>
    /// TranIconWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranIconWindow : WindowBase
    {
        public TranIconWindow(TranIconViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            Closing += TranIconWindow_Closing; ;

        }

        private void TranIconWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var wm = App.GetServices<IWindowManager>();
            wm.Hide(Models.WindowEnum.Tran);
            e.Cancel = true;
        }
    }
}
