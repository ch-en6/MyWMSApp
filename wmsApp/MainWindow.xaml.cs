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
using System.IO;
using Path = System.IO.Path;

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
            //窗口自适应
            Loaded += OnWindowLoaded;
         

            GenerateItems();


            // 添加导航菜单项的点击事件处理程序
            NavigationView.ItemInvoked += NavigationView_ItemInvoked;

            //设置默认选中在首页
            NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(item => item.Content.ToString() == "首页");

            // 设置初始导航页面
            ContentFrame.Navigate(new Uri("pages/HomePage.xaml", UriKind.Relative));

            // 注册Closing事件处理程序
            Closing += MainWindow_Closing;

          
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // 获取当前屏幕的工作区尺寸
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;

            // 设置窗口大小和位置
            Width = screenWidth;
            Height = screenHeight;
            Left = 0;
            Top = 0;
            WindowStartupLocation = WindowStartupLocation.Manual; // 设置窗口的启动位置为手动模式
        }


        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // 取消默认的窗口关闭操作

            // 显示提示框询问用户是否确认关闭窗口
            ContentDialog dialog = new ContentDialog()
            {
                Title = "提示",
                Content = "确定要关闭窗口退出登录吗？",
                SecondaryButtonText = "取消",
                PrimaryButtonText = "确定"
            };

            // 在 UI 线程上显示对话框并等待用户的响应
            ContentDialogResult result =await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Result logoutResult = LoginApi.logout();
                if (logoutResult.success)
                {
                    ModernMessageBox.showMessage("退出成功");
                }
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
                Tag = "pages/UserPage.xaml",  // 设置标签
                Width = 60, // 设置宽度
                Uri = "/userInfo",
                FontSize = 22
            };

            var item2 = new MyNavigationViewItem
            {
                Content = "首页",  // 设置内容
                Tag = "pages/HomePage.xaml",  // 设置标签
                Uri="/home",
                FontSize = 22
            };
            menuItems.Add(item1);
            menuItems.Add(item2);
      
            Result result = ResourceApi.getUserResources();
        
            Dictionary<String,Resource> map =JsonHelper.ConvertToMap<String, Resource>(result.data.ToString());
            foreach (var entry in map)
            {

                string key = entry.Key;  // 获取键
                Resource value = entry.Value;  // 获取值
                if (value.name == "用户中心") continue;
                MyNavigationViewItem item =new MyNavigationViewItem
                {
                    Content = value.name,
                    Tag = value.page,
                    Uri = value.uriName,
                    FontSize =22
                };
                menuItems.Add(item);

            }
            // 将MenuItems集合与NavigationView关联起来
            NavigationView.MenuItemsSource = menuItems;
        }
        private async void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
        {
       
            // 进行文件存在性检查
            string selectedPage = args.InvokedItemContainer.Tag.ToString();
            bool fileExists = File.Exists("../../"+selectedPage);
            if (fileExists)
            {
                // 获取自定义的URI属性
                MyNavigationViewItem selectedItem = args.InvokedItemContainer as MyNavigationViewItem;
                if (selectedItem != null)
                {
                    string uri = selectedItem.Uri;
                        if (uri == "/home")
                        {
                            ContentFrame.Navigate(new Uri(selectedPage, UriKind.Relative));
                            return;
                        }
                        try
                        {
                            Result result = PermissionApi.enter(uri);
                            if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                            // 根据Tag更改Frame的导航
                            if (result.success) ContentFrame.Navigate(new Uri(selectedPage, UriKind.Relative));
                            else ModernMessageBox.showMessage(result.errorMsg);
                        }
                        catch (TokenExpiredException ex)
                        {
                            Close();
                        };
                }
            }
            else
            {
                ModernMessageBox.showMessage("模块未开发");
                return;
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
