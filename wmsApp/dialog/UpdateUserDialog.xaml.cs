
using ModernWpf.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wmsApp.controls;
namespace wmsApp.dialog
{
    public sealed partial class UpdateUserDialog : ContentDialog
    {
        public UpdateUserDialog()
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
            //点击确认按钮时执行的逻辑
            //try
            //{
            // 获取用户输入的数据
            long id = long.Parse(userIdTextBlock.Text);
            string name = nameTextBox.Text;
            string role = sexComboBox.Text;
            if (role == "用户")
                role = "user";
            else
                role = "admin";
            string sex = sexComboBox.Text;
            string selectedDate = birthdatePicker.Text;
            DateTime date = DateTime.Parse(selectedDate);
            string idNumber = idNumberTextBox.Text;
            string nativePlace = nativePlaceTextBox.Text;
            string address = addressTextBox.Text;
            string phone = phoneTextBox.Text;

            User user = new User(id,name, role, sex, date, idNumber, nativePlace, address, phone);
            Result result = UserApi.update(user);

            if (result != null)
            {
                if (result.success)
                {
                    MessageBox.Show("修改成功");
                    // 关闭对话框
                    args.Cancel = false;
                    
                }

                else
                {
                    MessageBox.Show(result.errorMsg);
                    args.Cancel = true;
                }
            }

            //if (!result.success)
            //{
            //    throw new Exception(result.errorMsg.ToString());
            //}

            //}
            //catch (Exception ex)
            //{
            //    //处理异常
            //    args.Cancel = true;
            //}
        }


        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 点击取消按钮时执行的逻辑
            // 关闭对话框
            args.Cancel = false;
        }

        private void idNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            string idNumber = idNumberTextBox.Text;

            string pattern = @"^\d{17}[\dXx]$";

            if (Regex.IsMatch(idNumber, pattern))
            {
                idNumberErrorTextBlock.Text = ""; // 清空错误信息
            }
            else
            {
                idNumberErrorTextBlock.Text = "身份证号格式错误";
            }
        }

        private void phoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            string phone = phoneTextBox.Text;

            string pattern = "^1([38][0-9]|4[579]|5[0-3,5-9]|6[6]|7[0135678]|9[89])\\d{8}$";

            if (Regex.IsMatch(phone, pattern))
            {

                phoneTextBlock.Text = ""; // 清空错误信息
            }
            else
            {
                phoneTextBlock.Text = "手机号格式错误";
            }
        }

    }
}
