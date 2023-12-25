using ModernWpf.Controls;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wmsApp.utils;
using static wmsApp.utils.RSA;

namespace wmsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            // 订阅程序退出事件
            Exit += App_Exit;
            //异常退出
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //更新程序
            //Upgrade();
            //弹出登录框
            LoginWindow window = new LoginWindow();
            window.Show();
        }

        private async void Upgrade()
        {
            UpgradeWindow updateWindow = null;
            // 确保更新完成后启动应用程序
            try
            {
                /**
                 * 192.168.200.138:80 IP为服务器IP，端口为服务器端口号，"Setup.exe"是更新文件
                 */
                using (var manager = new UpdateManager("http://10.22.38.112:80", "Setup.exe"))
                {

                    var updateInfo = await manager.CheckForUpdate();

                    //判断是否有更新
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        var result = MessageBox.Show("检测到有新版本，是否更新?", "更新提示", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            updateWindow = new UpgradeWindow();
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

                            await manager.DownloadReleases(updateInfo.ReleasesToApply, progress);

                            await manager.ApplyReleases(updateInfo, progress);

                            updateWindow.Close();
                            MessageBox.Show("更新完毕，请重新启动");
                            System.Windows.Forms.Application.Restart();
                            System.Windows.Application.Current.Shutdown();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                updateWindow.Close();
                MessageBox.Show("更新完毕，请重新启动");
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            // 在此处添加程序退出时的操作
            // 例如保存数据、清理资源等
            TokenManager.token = "null";
            TokenManager.userId = 0;
            Result result = LoginApi.logout();
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
