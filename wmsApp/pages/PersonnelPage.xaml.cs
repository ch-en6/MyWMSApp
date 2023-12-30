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

namespace wmsApp.pages
{
    /// <summary>
    /// PersonalPage.xaml 的交互逻辑
    /// </summary>
    public partial class PersonalPage : System.Windows.Controls.Page
    {
        private int currentPage = 1;
        private int itemsPerPage = 2; // 每页显示10条数据
        private long totalPage;
        private int col_num;
        private Dictionary<string, string> resources;
        private long resourceId;
        //标记展示
        int flag = 0;

        public PersonalPage()
        {
            flag = 0;
            Result result = UserApi.search(currentPage);
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            List<User> userList = JsonHelper.JsonToList<User>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            dataGrid.ItemsSource = userList;

        }

        private Result searchByName(string userInput)
        {
            if (userInput == "")
            {
                userInput = "...";
            }
            Result result = UserApi.searchByName(currentPage, userInput);
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            return result;
        }

        private Result searchById(string userInput)
        {
            long id;
            long.TryParse(userInput, out id);
            Result result = UserApi.searchById(currentPage, id);
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            return result;
        }

        public void UpdatePage()
        {
            Result result;
            List<User> userList = null;

            switch (flag)
            {
                case 0:
                    result = UserApi.search(currentPage);
                    if (!result.success)
                    {
                        ModernMessageBox.showMessage(result.errorMsg);
                    }
                    userList = JsonHelper.JsonToList<User>(result.data.ToString());
                    break;
                case 1:
                    result = searchByName(textBox.Text);
                    if (!result.success)
                    {
                        ModernMessageBox.showMessage(result.errorMsg);
                    }
                    userList = JsonHelper.JsonToList<User>(result.data.ToString());
                    break;
            }
            PageNumberTextBlock.Text = currentPage.ToString();
            dataGrid.ItemsSource = userList;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                UpdatePage();
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddUserDialog dialog = new AddUserDialog();

                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Secondary) return;

                UpdatePage();

            }
            catch (Exception ex)
            {
                ModernMessageBox.showMessage(ex.Message);
            }
        }

        public void resetPassword_Click(object sender, RoutedEventArgs e)
        {
            // 获取按钮所在行
            Button resetPassword = (Button)sender;
            User user = (User)resetPassword.DataContext;

            Result result = UserApi.resetPassword(user);
            if (result.success)
            {
                MessageBox.Show("重置成功");
            }
            else
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // 获取按钮所在行
            Button deleteButton = (Button)sender;
            User user = (User)deleteButton.DataContext;

            // 提取ID值
            long id = user.id;

            Result result = UserApi.delete(id);

            if (result.success)
            {
                MessageBox.Show("删除成功");
            }
            else
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            UpdatePage();

        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = (Button)sender;
            User user = (User)updateButton.DataContext;

            // 提取ID值
            //long id = user.id;
            UpdateUserDialog dialog = new UpdateUserDialog();

            // 将传递过来的数据填充到对应的控件中
            dialog.userIdTextBlock.Text = user.id.ToString();
            dialog.nameTextBox.Text = user.name;
            dialog.roleTextBox.SelectedIndex = user.role == "管理员" ? 1 : 0; // 根据role值设置ComboBox的选中项
            dialog.sexComboBox.SelectedIndex = user.sex == "女" ? 1 : 0; // 根据sex值设置ComboBox的选中项
            dialog.birthdatePicker.SelectedDate = user.birthDate;
            dialog.idNumberTextBox.Text = user.idNumber;
            dialog.nativePlaceTextBox.Text = user.nativePlace;
            dialog.addressTextBox.Text = user.address;
            dialog.phoneTextBox.Text = user.phone;
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary) return;
            UpdatePage();
        }






        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 防止事件继续传递
                e.Handled = true;

                // 触发搜索按钮的点击事件
                Button_Click(sender, e);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
            string selectedQueryType = selectedComboBoxItem.Name;
            string userInput = textBox.Text;
            currentPage = 1;
            Result result = null;
            List<User> userList;

            switch (selectedQueryType)
            {
                case "userName":
                    flag = 1;
                    result = searchByName(userInput);
                    break;
                case "userId":
                    flag = 2;
                    result = searchById(userInput);
                    break;

            }
            userList = JsonHelper.JsonToList<User>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            dataGrid.ItemsSource = userList;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //IsSearching = false;
            currentPage = 1;
            UpdatePage();
        }
    }
}