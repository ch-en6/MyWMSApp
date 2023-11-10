using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wmsApp.controls;

namespace wmsApp
{
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException() : base()
        {
            MessageBox.Show("请重新登录");
            // 获取 Frame 所在的 Window
            Window window = Application.Current.MainWindow;

            if (window != null)
            {
               
                // 关闭 Window
                window.Close();

                // 弹出登录窗口
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        }
    }
    
}
