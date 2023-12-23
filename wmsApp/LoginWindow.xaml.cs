using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.utils;
using static wmsApp.utils.RSA;

namespace wmsApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.KeyDown += UsernameTextBox_KeyDown;
            PasswordBox.KeyDown += PasswordBox_KeyDown;
            Loaded += OnWindowLoaded;
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


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // 防止回车键产生默认行为
                UsernameTextBox.Focus();
            }
        }
        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // 防止回车键产生默认行为
                PasswordBox.Focus();
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // 防止回车键产生默认行为
                LoginButton_Click(sender, new RoutedEventArgs());
            }
        }

        private async Task<Result> PerformLogin(long userId, string password)
        {
         
            ShowLoadingOverlay(); // 显示蒙版和设置 ProgressRing 的 IsActive 属性为 true

            //生成密钥
            GenerateKey();
            //发送登录请求
            return await Task.Run(() => LoginApi.login(userId, password));
        
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            long userId;
            if (long.TryParse(username, out userId))
            {
                
                //发送登录请求
                Task<Result> loginTask = PerformLogin(userId, password);

                // 等待登录结果
                loginTask.ContinueWith(task =>
                {
                    Result result = task.Result;
                    if (result != null && result.success)
                    {
                        string token = result.data.ToString();
                        TokenManager.token = token;
                        TokenManager.userId = userId;

                        // 登录成功，打开主窗口,获取密钥
                        Dispatcher.Invoke(() =>
                        {
                            //打开主窗口
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();

                            Close();
                        });
                    }
                    HideLoadingOverlay(); // 隐藏蒙版和设置 ProgressRing 的 IsActive 属性为 false
                });
            }
            else
            {
                MessageBox.Show("用户名格式不正确");
            }
        }

        private void ShowLoadingOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                loadingGrid.Visibility = Visibility.Visible;
                ring.IsActive = true;
            });
        }

        private void HideLoadingOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                loadingGrid.Visibility = Visibility.Collapsed;
                ring.IsActive = false;
            });
        }

        private bool IsValidCredentials(string username, string password)
        {
            long userId;
            // 进行实际的用户名和密码验证逻辑
            if (username != null  && password !=null )
            {
                if (!long.TryParse(username, out userId))
                { 
                    MessageBox.Show("用户名格式不正确！");
                    return false;
                }
                return true;
            }
            else
            {
                MessageBox.Show("用户名和密码不能为空！");
                return false;
            }
        }
    }
}
