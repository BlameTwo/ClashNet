using H.NotifyIcon;
using System;
using System.Windows.Media.Imaging;

namespace ClashNet;

public class Run
{
    private static bool _contentLoaded;

    [STAThread]
    public static void Main(string[] args)
    {
        if (_contentLoaded)
        {
            return;
        }
        _contentLoaded = true;
        App app = new App();
        System.Uri resourceLocater = new System.Uri("/ClashNet;component/app.xaml", System.UriKind.Relative);
        System.Windows.Application.LoadComponent(app, resourceLocater);
        app.Run();
    }
}
