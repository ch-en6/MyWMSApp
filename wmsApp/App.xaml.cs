﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wmsApp.controls;
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
             StartupUri = new Uri("LoginWindow.xaml", UriKind.Relative);
            // 订阅程序退出事件
            Exit += App_Exit;
            //异常退出
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            /*// 在此处添加程序退出时的操作
            // 例如保存数据、清理资源等

            // 退出前的操作完成后，程序将正常退出
            TokenManager.token = "null";
            TokenManager.userId = 0;
            Result result =LoginApi.logout();
            if (result.success) await ModernMessageBox.Show("提示","退出成功");*/
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                // 退出登录
                TokenManager.token = "null";
                TokenManager.userId = 0;
                LoginApi.logout();
                // 关闭应用程序
                Current.Shutdown(-1); // 传入非零值表示应用程序异常退出
            }
        }
    }
}
