using ModernWpf.Controls;
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

namespace wmsApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //设置默认选中在首页
            NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(item => item.Content.ToString() == "首页");

            // 添加导航菜单项的点击事件处理程序
            NavigationView.ItemInvoked += NavigationView_ItemInvoked;

            // 设置初始导航页面
            ContentFrame.Navigate(new Uri("pages/HomePage.xaml", UriKind.Relative));
        }
        private void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // 处理“设置”项的点击事件
                // ...
            }
            else
            {
                // 获取点击的导航菜单项的Tag
                string selectedPage = args.InvokedItemContainer.Tag.ToString();

                // 根据Tag更改Frame的导航
                ContentFrame.Navigate(new Uri(selectedPage, UriKind.Relative));
            }
        }

    }
}
