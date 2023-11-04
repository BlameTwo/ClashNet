using ClashNet.Services;
using ClashNet.ViewModels;
using SimpleUI.Controls;
using System;

namespace ClashNet;

public partial class MainWindow : WindowBase
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
        Loaded += MainWindow_Loaded;
        Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var wm = App.GetServices<IWindowManager>();
        wm.Hide(Models.WindowEnum.Main);
        e.Cancel = true;
    }

    private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if(this.DataContext is MainViewModel vm)
        {
            vm.AppNavigationView.Init(this.framebase, true);
            vm.ToastLitterMessage.ShowOwner = this.grid;
            vm.ToastLitterMessage.ShowTime = TimeSpan.FromSeconds(3);
        }
    }
}
