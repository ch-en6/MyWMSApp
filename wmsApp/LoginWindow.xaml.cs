﻿using System;
using System.Threading.Tasks;
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
        private async Task<Result> PerformLogin(long userId, string password)
        {
         
                ShowLoadingOverlay(); // 显示蒙版和设置 ProgressRing 的 IsActive 属性为 true

                return await Task.Run(() => LoginApi.login(userId, password));
        
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            long userId;
            if (long.TryParse(username, out userId))
            {
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

                        // 登录成功，打开主窗口或执行其他操作
                        Dispatcher.Invoke(() =>
                        {
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
