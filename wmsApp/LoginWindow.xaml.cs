using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.controls;
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
          /*  Loaded += OnWindowLoaded;*/
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HintTextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 将焦点设置到PasswordBox
            Keyboard.Focus(PasswordBox);
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

/*        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // 获取当前屏幕的工作区尺寸
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;

            // 设置窗口位置
            Left = (screenWidth - ActualWidth) / 2;
            Top = (screenHeight - ActualHeight) / 2;
            WindowStartupLocation = WindowStartupLocation.Manual; // 设置窗口的启动位置为手动模式
        }

*/

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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 点击登录按钮后先禁用输入框和登录按钮
            UsernameTextBox.IsEnabled = false;
            PasswordBox.IsEnabled = false;
            LoginButton.IsEnabled = false;

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            long userId;
            if (long.TryParse(username, out userId))
            {
                //发送登录请求
                Task<Result> loginTask = PerformLogin(userId, password);

                // 等待登录结果
                Result result = await loginTask;
                if (result != null && result.success)
                {
                    string token = result.data.ToString();
                    TokenManager.token = token;
                   /* TokenManager.userId = userId;*/

                    // 登录成功，打开主窗口
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    // 关闭当前窗口
                    this.Close();
                }
                else
                {
                    ModernMessageBox.showMessage("登录失败！");
                }
            }
            else
            {
                ModernMessageBox.showMessage("用户名格式不正确");
            }

            // 启用输入框和登录按钮
            UsernameTextBox.IsEnabled = true;
            PasswordBox.IsEnabled = true;
            LoginButton.IsEnabled = true;
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
                    ModernMessageBox.showMessage("用户名格式不正确！");
                    return false;
                }
                return true;
            }
            else
            {
                ModernMessageBox.showMessage("用户名和密码不能为空！");
                return false;
            }
        }
    }
}
