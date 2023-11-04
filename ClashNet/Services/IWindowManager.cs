using ClashNet.Models;
using System.Windows;

namespace ClashNet.Services;

public interface IWindowManager
{

    public void InitWindow<T>(WindowEnum wenum,T window)
        where T:Window;


    bool TranShow { get; set; }

    bool MainShow { get; set; }

    bool LookbackShow { get; set; }

    void Show(WindowEnum wenum);

    void Hide(WindowEnum wenum);
}
