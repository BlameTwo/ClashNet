using ClashNet.Services;
using ClashNet.ViewModels.Windows;
using SimpleUI.Controls;

namespace ClashNet.Views.Windows;

public partial class WindowLookBack : WindowBase
{
    public WindowLookBack(LookBackViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
        Closing += WindowLookBack_Closing;
    }

    private void WindowLookBack_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var wm = App.GetServices<IWindowManager>();
        wm.Hide(Models.WindowEnum.Lookback);
        e.Cancel = true;
    }
}
