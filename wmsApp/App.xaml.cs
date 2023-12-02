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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 确保更新完成后启动应用程序
            /*            
                            192.168.200.137:80 为web服务器地址
                            到时候建立本机与虚拟机端口映射，设置为本机ip地址
                        */
  /*          try
            {
                using (var manager = new UpdateManager("http://10.22.33.107:80", "Setup.exe"))
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
                        var updateInfo = await manager.CheckForUpdate();
                        Dictionary<ReleaseEntry, string> map = updateInfo.FetchReleaseNotes();

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
            MainWindow window = new MainWindow();
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
