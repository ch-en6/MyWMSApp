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
using WindowsFormsApp1.dto;
using wms;
using wmsApp;
using wms.utils;
using wmsApp.pojo;
using wmsApp.controls;

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
            GenerateItems();
            // 添加导航菜单项的点击事件处理程序
            NavigationView.ItemInvoked += NavigationView_ItemInvoked;

            // 设置初始导航页面
            ContentFrame.Navigate(new Uri("pages/HomePage.xaml", UriKind.Relative));
        }

        private void GenerateItems()
        {
            // 创建一个MenuItems集合
            var menuItems = new List<MyNavigationViewItem>();
            // 创建第一个 NavigationViewItem
            var item1 = new MyNavigationViewItem
            {
                Content = "",  // 设置内容
                Icon = new SymbolIcon(Symbol.Contact),  // 设置图标
                Tag = "pages/UserPage.xaml",  // 设置标签
                Width = 48 , // 设置宽度
                Uri = ""
            };

            // 创建第二个 NavigationViewItem
            var item2 = new MyNavigationViewItem
            {
                Content = "首页",  // 设置内容
                Tag = "pages/HomePage.xaml",  // 设置标签
                Uri=""
            };
            menuItems.Add(item1);
            menuItems.Add(item2);

            Result result = ResourceApi.getResources();
            
            Dictionary<String,Resource> map =JsonHelper.ConvertToMap<String, Resource>(result.data.ToString());
            foreach (var entry in map)
            {

                string key = entry.Key;  // 获取键
                Resource value = entry.Value;  // 获取值
                MyNavigationViewItem item =new MyNavigationViewItem
                {
                    Content = value.name,
                    Tag = value.page,
                    Uri = value.uriName
                };
                menuItems.Add(item);

            }
            // 将MenuItems集合与NavigationView关联起来
            NavigationView.MenuItemsSource = menuItems;
        }
        private async void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
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


                // 获取自定义的URI属性
                MyNavigationViewItem selectedItem = args.InvokedItemContainer as MyNavigationViewItem;
                if (selectedItem != null)
                {
                    string uri = selectedItem.Uri;
                    if (uri == "")return ;
                    Result result = PermissionApi.enter(uri);
                    // 根据Tag更改Frame的导航
                    if (result.success) ContentFrame.Navigate(new Uri(selectedPage, UriKind.Relative));
                    else await ModernMessageBox.Show("提示",result.errorMsg);
                    
                }
            }
        }

/*        private void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
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
        }*/

    }
}
