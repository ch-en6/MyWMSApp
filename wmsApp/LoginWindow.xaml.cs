using System;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.utils;

namespace wmsApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // 在这里执行登录逻辑
            if (IsValidCredentials(username, password))
            {
                long userId = long.Parse(username);
                Result result = LoginApi.login(userId, password);
                if (!result.success)
                {
                    MessageBox.Show(result.errorMsg.ToString());
                    return;
                }
                string token = result.data.ToString();
                TokenManager.token = token;
                TokenManager.userId = userId;
                MessageBox.Show(token);
                // 登录成功，打开主窗口或执行其他操作
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                // 登录失败，显示错误提示
                MessageBox.Show("Invalid username or password.", "Login Failed");
            }
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
