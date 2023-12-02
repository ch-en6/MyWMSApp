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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 确保更新完成后启动应用程序
            /*            
                            192.168.200.137:80 为web服务器地址
                            到时候建立本机与虚拟机端口映射，设置为本机ip地址
            */
/*
            try
            {
                using (var manager = new UpdateManager("http://10.22.33.107:80","Setup.exe"))
                {

                    // 检查并安装更新
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => manager.CreateShortcutForThisExe(),
                        onAppUpdate: v => manager.CreateShortcutForThisExe(),
                        onAppUninstall: v => manager.RemoveShortcutForThisExe()
                    );

                    // 检查是否有更新可用
                    Task.Run(async () =>
                    {
                        await manager.UpdateApp();
                        var updateInfo = await manager.CheckForUpdate();
                        if (updateInfo.ReleasesToApply.Any())
                        {
                            // 下载并安装更新
                            var upgradeWindow = new UpgradeWindow(); // 创建 UpgradeWindow 对象
                            var progress = new Progress<int>(percent =>
                            {
                                upgradeWindow.UpdateProgress(percent); // 直接更新 UpgradeWindow 中的进度条
                        });
                            await manager.UpdateApp();
                            MessageBox.Show("更新完毕");
                            // 重新启动应用程序

                            System.Windows.Forms.Application.Restart();
                            System.Windows.Application.Current.Shutdown();

                        }
                    }).Wait();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
            // 生成密钥
            GenerateKey();
            // 启动应用程序的主窗口
            LoginWindow window = new LoginWindow();
            window.Show();
        }

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
