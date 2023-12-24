using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms;

namespace wmsApp.dialog
{
    /// <summary>
    /// UpdatePhoneDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePhoneDialog : ContentDialog
    {
        private string originalPhone;
        public UpdatePhoneDialog(String phone)
        {
            originalPhone=phone;
            this.InitializeComponent();
        }
        private void phoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            string newPhone = phoneTextBox.Text;

            string pattern = "^1([38][0-9]|4[579]|5[0-3,5-9]|6[6]|7[0135678]|9[89])\\d{8}$";

            if (Regex.IsMatch(newPhone, pattern))
            {
                if(newPhone==originalPhone)
                {
                    phoneTextBlock.Text = "新旧手机号一致";
                    IsPrimaryButtonEnabled = false;
                    codeButton.IsEnabled = false;
                    return;
                }
                phoneTextBlock.Text = ""; // 清空错误信息
                IsPrimaryButtonEnabled = true;
                codeButton.IsEnabled = true;
            }
            else
            {
                phoneTextBlock.Text = "手机号格式错误";
                IsPrimaryButtonEnabled = false ;
                codeButton.IsEnabled = false;

            }
        }

        private void Code_Click(object sender, RoutedEventArgs e)
        {
            // 获取按钮所在行
            string phone = phoneTextBox.Text;
            Result result = MsmApi.sendCode(phone);
            if (result.success)
            {
                codeTextBlock.Text = "已发送";
            }
            else
            {
                MessageBox.Show(result.errorMsg);
            }
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string phone = phoneTextBox.Text;
            string code = codeTextBox.Text;
            Result result = MsmApi.checkCode(phone, code);
            if (result.success)
            {
                Result result2 = UserInfoApi.updatePhone(phone);
                if (result2.success)
                {
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show(result2.errorMsg);
                }
            }
            else
            {
                MessageBox.Show(result.errorMsg);
                args.Cancel = true;
            }
        }


        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 点击取消按钮时执行的逻辑
            // 关闭对话框
            args.Cancel = false;
        }



    }
}
