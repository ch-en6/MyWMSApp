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
using System.Windows.Shapes;
using System.ComponentModel;
using wmsApp.controls;
using wms.utils;
using wms;
using WindowsFormsApp1.dto;

namespace wmsApp.dialog
{
    /// <summary>
    /// SelectPersonDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SelectPersonDialog : Window
    {
        //当前页
        int currentPage = 1;
        //总页数
        long totalPage = 1;
 
        int flag = 0;

        List<Person> selectedPersons;

        public delegate void ConfirmButtonClickedEventHandler(object sender, List<Person> selectedPersons);
        public event ConfirmButtonClickedEventHandler ConfirmButtonClicked;
        public delegate void CancelButtonClickedEventHandler(object sender);
        public event CancelButtonClickedEventHandler CancelButtonClicked;


        private TaskCompletionSource<Result> tcs;

        public new Task<Result> Show()
        {
            tcs = new TaskCompletionSource<Result>();

            ConfirmButtonClicked += OnConfirmButtonClicked;

            base.Show();

            return tcs.Task;
        }
        public SelectPersonDialog()
        {
            Result result = UserApi.search(currentPage);
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            List<Person> userList = JsonHelper.JsonToList<Person>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            userListView.ItemsSource = userList;
        }
        private void OnConfirmButtonClicked(object sender, List<Person> selectedPersons)
        {
            ConfirmButtonClicked -= OnConfirmButtonClicked;
         
            Result result = new Result
            {
                success = true,
                data = selectedPersons,
            };

            tcs.SetResult(result);
        }

        private void OnCancelButtonClicked(object sender)
        {
            ConfirmButtonClicked -= OnConfirmButtonClicked;
            CancelButtonClicked -= OnCancelButtonClicked;

            Result result = new Result
            {
                success = false,
            };

            tcs.SetResult(result);
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)SelectAllCheckBox.IsChecked)
            {
                Result result = PermissionApi.getAllUser();
                if (!result.success)
                {
                    MessageBox.Show("全选失败");
                    this.Hide();
                    return;
                }
                selectedPersons = JsonHelper.JsonToList<Person>(result.data.ToString());
                foreach(Person p in selectedPersons)
                {
                    p.IsSelected = true;
                }
            }
            else
            {
                selectedPersons = GetSelectedPersonsAsync();
            }

            ConfirmButtonClicked?.Invoke(this, selectedPersons);
            MessageBox.Show("已选择人员");
            this.Hide();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClicked?.Invoke(this);
            this.Hide();
        }
        private List<Person> GetSelectedPersonsAsync()
        {
           
                List<Person> selectedPersons = new List<Person>();
                // 遍历userListView的每个项，找出被选中的项
                foreach (Person person in userListView.Items)
                {
                    if (person.IsSelected)
                    {
                        selectedPersons.Add(person);
                    }
                }
                return selectedPersons;
       
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
            string selectedQueryType = selectedComboBoxItem.Name;
            string userInput = textBox.Text;
            currentPage = 1;
            Result result = null;
            List<Person> userList;

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
            userList = JsonHelper.JsonToList<Person>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString()+"/"+totalPage.ToString();
            userListView.ItemsSource = userList;
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 防止事件继续传递
                e.Handled = true;

                // 触发搜索按钮的点击事件
                SearchButton_Click(sender, e);
            }
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in userListView.Items)
            {
                if (item is Person person)
                {
                    person.IsSelected = true;
                }
            }
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in userListView.Items)
            {
                if (item is Person person)
                {
                    person.IsSelected = false;
                }
            }
        }


        public void UpdatePage()
        {
            Result result;
            List<Person> userList = null;

            switch (flag)
            {
                case 0:
                    result = UserApi.search(currentPage);
                    if (!result.success)
                    {
                        ModernMessageBox.showMessage(result.errorMsg);
                    }
                    userList = JsonHelper.JsonToList<Person>(result.data.ToString());
                    break;
                case 1:
                    result = searchByName(textBox.Text);
                    if (!result.success)
                    {
                        ModernMessageBox.showMessage(result.errorMsg);
                    }
                    userList = JsonHelper.JsonToList<Person>(result.data.ToString());
                    break;
            }
            PageNumberTextBlock.Text = currentPage.ToString();
            userListView.ItemsSource = userList;
            if ((bool)SelectAllCheckBox.IsChecked) SelectAllCheckBox_Checked(null, null);
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
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePage();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                UpdatePage();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            flag = 0;
            UpdatePage();
        }

    }
    public class Person : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        private long _id;
        private string _role;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged("Role");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}