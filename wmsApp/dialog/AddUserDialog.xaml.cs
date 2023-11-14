
using ModernWpf.Controls;
using System;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wmsApp.controls;
namespace wmsApp.dialog
{
    public sealed partial class AddUserDialog : ContentDialog
    {
        public AddUserDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 点击确认按钮时执行的逻辑
            try
            {
                // 获取用户输入的数据
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

                User user = new User(name, role, sex, date, idNumber, nativePlace, address, phone);
                Result result = UserApi.save(user);
                if (result != null)
                {
                    if (result.success)
                        MessageBox.Show("添加成功");
                    else MessageBox.Show(result.errorMsg);
                }
                // 关闭对话框
                args.Cancel = false;
            }
            catch (Exception ex)
            {
                // 处理异常
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
