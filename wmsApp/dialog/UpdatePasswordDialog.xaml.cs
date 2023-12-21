
using ModernWpf.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wmsApp.controls;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.dialog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace wmsApp.dialog
{
    public sealed partial class UpdatePasswordDialog : ContentDialog
    {
        public UpdatePasswordDialog()
        {
            this.InitializeComponent();
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string phone = phoneTextBlock.Text;
            string code = codeTextBox.Text;
            Result result = MsmApi.checkCode(phone,code);
            if(result.success)
            {
                MessageBox.Show("修改成功！");
            }
            else
            {
                MessageBox.Show(result.errorMsg);
            }
        }


        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 点击取消按钮时执行的逻辑
            // 关闭对话框
            args.Cancel = false;
        }

        private void newPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string confirmPassword = confirmPasswordBox.Password;
            string newPassword = newPasswordBox.Password;
            if (string.IsNullOrEmpty(newPassword))
            {
                newPasswordTextBlock.Text = "不能为空";
            }
            else
            {
                newPasswordTextBlock.Text = "";
            }
            if (confirmPassword != newPassword)
            {
                confirmPasswordTextBlock.Text = "两次密码不一致";
            }
            UpdateConfirmButtonState();
        }

        private void confirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string confirmPassword = confirmPasswordBox.Password;
            string newPassword = newPasswordBox.Password;
            if (string.IsNullOrEmpty(confirmPassword))
            {
                confirmPasswordTextBlock.Text = "不能为空";
            }
            else if (confirmPassword != newPassword)
            {
                confirmPasswordTextBlock.Text = "两次密码不一致";
            }
            else
            {
                confirmPasswordTextBlock.Text = "";
            }
            UpdateConfirmButtonState();
        }

        private void UpdateConfirmButtonState()
        {
            if (string.IsNullOrEmpty(newPasswordBox.Password) || string.IsNullOrEmpty(confirmPasswordBox.Password) || confirmPasswordBox.Password != newPasswordBox.Password)
            {
                IsPrimaryButtonEnabled = false;
            }
            else
            {
                IsPrimaryButtonEnabled = true;
            }
            
        }


       
        private void Code_Click(object sender, RoutedEventArgs e)
        {
            // 获取按钮所在行
            string phone = phoneTextBlock.Text;
            Result result = MsmApi.sendCode(phone);
            if(result.success)
            {
                codeTextBlock.Text = "已发送";
            }
            else
            {
                MessageBox.Show(result.errorMsg);
            }
        }
        
     }
}
