using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using wms;
using wmsApp.utils;

namespace wmsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
           /* if(TokenManager.token==null)StartupUri = new Uri("LoginWindow.xaml", UriKind.Relative);*/
             StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            // 订阅程序退出事件
            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            // 在此处添加程序退出时的操作
            // 例如保存数据、清理资源等

            // 退出前的操作完成后，程序将正常退出
            TokenManager.token = null;
            TokenManager.userId = 0;
            LoginApi.logout();
        }


    }
}
