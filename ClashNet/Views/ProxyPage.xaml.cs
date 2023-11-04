using ClashNet.ViewModels;
using SimpleUI.Controls;
using SimpleUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClashNet.Views
{
    /// <summary>
    /// ProxyPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProxyPage : Page, IPage
    {
        public ProxyPage(ProxyViewModel vm)
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
}
