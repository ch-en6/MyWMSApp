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
using wmsApp.utils;

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
            NavigationView navigation;

            // 设置初始导航页面
            ContentFrame.Navigate(new Uri("pages/HomePage.xaml", UriKind.Relative));

            // 注册Closing事件处理程序
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 显示提示框询问用户是否确认关闭窗口
            MessageBoxResult result = MessageBox.Show("确定要关闭窗口吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // 如果用户点击“是”，则关闭窗口；否则取消关闭操作
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // 取消关闭操作
            }
            else
            {
                Result result1 = LoginApi.logout();
                if (result1.success) MessageBox.Show("退出成功"); 
                TokenManager.token = "null";
                TokenManager.userId = 0;
                Application.Current.Shutdown(); // 触发应用程序的 Exit
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
             Close();
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
                //Tag = "pages/UserPage.xaml",  // 设置标签
                Width = 60 , // 设置宽度
                Uri = "",
                FontSize = 22
            };

            // 创建第二个 NavigationViewItem
            var item2 = new MyNavigationViewItem
            {
                Content = "首页",  // 设置内容
                Tag = "pages/HomePage.xaml",  // 设置标签
                Uri="",
                FontSize = 22
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
                    Uri = value.uriName,
                    FontSize =22
                };
                if (value.name == "用户中心")
                {
                    item1.Tag = value.page;
                    item1.Uri = value.uriName; 
                  
                    continue;
                }
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
                    try {
                    Result result = PermissionApi.enter(uri);
                        if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                        // 根据Tag更改Frame的导航
                        if (result.success) ContentFrame.Navigate(new Uri(selectedPage, UriKind.Relative));
                        else ModernMessageBox.showMessage(result.errorMsg);
                    }catch(TokenExpiredException ex) {
                        Close();
                    };


                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
