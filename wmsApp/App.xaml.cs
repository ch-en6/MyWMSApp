using System;
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
using Squirrel;
using ModernWpf.Controls;
using static wmsApp.utils.RSA;

namespace wmsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 确保更新完成后启动应用程序
            try
            {
                /**
                 * 192.168.200.138:80 IP为服务器IP，端口为服务器端口号，"Setup.exe"是更新文件
                 */
               /* using (var manager = new UpdateManager("http://10.22.33.107:80", "Setup.exe"))
                {
            
                    var updateInfo = await manager.CheckForUpdate();

                    //判断是否有更新
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        var updateWindow = new UpgradeWindow();
                        updateWindow.Show();

                        var progress = new Action<int>(value =>
                        {
                            updateWindow.Dispatcher.Invoke(() =>
                            {
                                // 更新进度条
                                updateWindow.UpdateProgress(value);
                            });
                        });
                        await manager.UpdateApp(progress);
                     
                        *//*await manager.DownloadReleases(updateInfo.ReleasesToApply, progress);
                        await manager.ApplyReleases(updateInfo, progress);*//*
                      
                        updateWindow.Dispatcher.Invoke(() =>
                        {
                            updateWindow.Close();
                            MessageBox.Show("更新完毕，请重新启动");
                            System.Windows.Forms.Application.Restart();
                            System.Windows.Application.Current.Shutdown();
                        });
                    }
                }*/
            }
            catch (Exception ex)
            {
            
            }

            //生成rsa密钥
            GenerateKey();
            /*
                  RsaApi.test();*/
            //弹出登录框
            LoginWindow window = new LoginWindow();
            window.Show();
        }


        public App()
        {
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
        private void GenerateKey()
        {
            //生成前端RSA密钥
            RSA rsa = new RSA();
            RSAKEY Rsakey = rsa.GetKey();
            TokenManager.csKey = new Dictionary<string, string>();
            TokenManager.csKey["publickey"] = Rsakey.PublicKey;
            TokenManager.csKey["privatekey"] = Rsakey.PrivateKey;
  
            //获取后端RSA公钥
            String key = RsaApi.getJavaPublicKey();
            TokenManager.javaPublicKey = key;
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
