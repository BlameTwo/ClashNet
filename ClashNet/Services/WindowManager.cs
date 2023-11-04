using System;
using System.Windows;
using System.Windows.Interop;
using ClashNet.ViewModels.Windows;
using ClashNet.Models;
using Serilog;

namespace ClashNet.Services;

public class WindowManager : IWindowManager
{
    public WindowManager(ILogger logger)
    {
        Logger = logger;
    }

    const int WM_SYSCOMMAND = 0x0112;
    const int SC_MOVE = 0xF010;
    void RefSize()
    {
        TranIcon.Left = SystemParameters.PrimaryScreenWidth - TranIcon.ActualWidth; ;
        TranIcon.Top = SystemParameters.WorkArea.Height - TranIcon.ActualHeight;
    }

    private void TranIcon_SourceInitialized(object? sender, EventArgs e)
    {
        WindowInteropHelper helper = new WindowInteropHelper(TranIcon);
        HwndSource source = HwndSource.FromHwnd(helper.Handle);
        source.AddHook(WndProc);

    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_SYSCOMMAND:
                int command = wParam.ToInt32() & 0xfff0;
                if (command == SC_MOVE)
                {
                    handled = true;
                }
                break;
            default:
                break;
        }
        return IntPtr.Zero;
    }

    public bool TranShow { get; set; }
    public bool MainShow { get; set; }

    public bool LookbackShow { get; set; }

    Window TranIcon { get; set; }

    Window MainWindow { get; set; }

    Window LookBackWindow { get; set; }
    public ILogger Logger { get; }

    public void Hide(WindowEnum wenum)
    {
        Logger.Information($"隐藏：{wenum}");
        switch (wenum)
        {
            case WindowEnum.Tran:
                RefSize();
                if (TranIcon == null) break;
                TranIcon.Hide();
                this.TranShow = false;
                break;
            case WindowEnum.Main:
                if (MainWindow == null) break;
                MainWindow.Hide();
                MainShow = false;
                break;
            case WindowEnum.Lookback:
                if (LookBackWindow == null) break;
                LookBackWindow.Hide();
                LookbackShow = false;
                break;
        }
    }


    public void Show(WindowEnum wenum)
    {
        Logger.Information($"显示：{wenum}");
        switch (wenum)
        {
            case WindowEnum.Tran:
                RefSize();
                if (TranIcon != null)
                {
                    TranIcon.Show();
                    (TranIcon.DataContext as TranIconViewModel)!.ShowMethod();
                    TranShow = true;
                }
                break;
            case WindowEnum.Main:
                if (MainWindow != null)
                {
                    MainWindow.Show();
                    MainShow = true;
                }
                break;
            case WindowEnum.Lookback:
                if (LookBackWindow == null) break;
                LookBackWindow.Show();
                LookbackShow = true;
                break;
        }
    }

    public void InitWindow<T>(WindowEnum wenum, T window)
        where T:Window
    {

        Logger.Information($"注册：{window.GetType()}");
        if (wenum == WindowEnum.Tran)
        {
            this.TranIcon = window;
            TranIcon.SourceInitialized += TranIcon_SourceInitialized;
            TranIcon.WindowStartupLocation = WindowStartupLocation.Manual;
            TranIcon.Loaded += (s, e) =>
            {
                RefSize();
            };
            TranIcon.Focus();
            TranIcon.Deactivated += (s, e) =>
            {
                Hide(WindowEnum.Tran);
            };
            TranShow = false;
        }
        else if (wenum == WindowEnum.Main)
        {
            this.MainWindow = window;
        }else if(wenum == WindowEnum.Lookback)
        {
            this.LookBackWindow = window;
            LookBackWindow.Focus();
            LookbackShow = false;
        }
    }
}
