using ModernWpf.Controls;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.UI.MobileControls;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wms.param;
using System;
using wmsApp.controls;

namespace wmsApp.dialog
{
    public  partial class PermissionDialog : ContentDialog, INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }

        long resourceId;

        Dictionary<string, string> resources;

        Dictionary<String, long> types;

        List<User> userList;
        public PermissionDialog(Dictionary<string, string> resources,long resourceId)
        { 
            this.resources = resources;
            this.resourceId = resourceId;
            Result result = PermissionApi.getUserNamesAndIds();
            if (!result.success)
            {
                throw new Exception(result.errorMsg.ToString());
            }
            Result result1 = PermissionTypesApi.getSelectMap(resourceId);
            if (!result1.success)
            {
                throw new Exception(result.errorMsg.ToString());
            }

            types = JsonHelper.ConvertToMap<String, long>(result1.data.ToString());

            userList = JsonHelper.JsonToList<User>(result.data.ToString());
            InitializeComponent();
           
            initPeople();
            initResource();
            initTypes();
            DataContext = this;
        }
        private void initPeople()
        {
            // 初始化人员列表数据源
            People = new ObservableCollection<Person>();
            foreach (User user in userList)
            {
                Person p = new Person { Name = user.id + " " + user.name, IsSelected = false, Id = user.id };
                People.Add(p);
            }
        }

        private void initResource()
        {
            foreach (KeyValuePair<string, string> kvp in resources)
            {
                string key = kvp.Key;
                string value = kvp.Value;

                if(value == resourceId.ToString())
                {
                    resourceComboBox.Text = key;
                }
            }
        }

        private void initTypes()
        {
            List<String> items = new List<string>();
            foreach(KeyValuePair<String, long> kvp in types)
            {
                string key = kvp.Key;
                items.Add(key);
            }
            typeTextBox.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 点击 "选择人员" 按钮时的逻辑处理
            userListView.Visibility = userListView.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
   
        }
        private bool ValidateFields()
        {
            // 验证资源名称是否为空
            if (string.IsNullOrWhiteSpace(resourceComboBox.Text))
            {
                MessageBox.Show("请填写资源名称");
                return false;
            }

            // 验证权限名称是否为空
            if (string.IsNullOrWhiteSpace(typeTextBox.Text))
            {
                MessageBox.Show("请填写权限名称");
                return false;
            }


            return true;
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool isValid = ValidateFields();
            if (!isValid) return;
            // 点击 "Save" 按钮时的逻辑处理
            // 遍历 People 集合，检查每个人员的 IsSelected 属性来确定用户选择了哪些人员
            List<long> userIds = new List<long>();
            foreach (Person person in People)
            {
                if (person.IsSelected)
                {
                    // 处理选中的人员
                    userIds.Add(person.Id);
                    
                }
            }

            string type = typeTextBox.Text;
            string permissionName = typeTextBox.Text;
           
            AddPermissionParams param = new AddPermissionParams(userIds,types[permissionName], type);
            Result result = PermissionApi.savePermissions(param);
            if (!result.success)
            {
                MessageBox.Show(result.errorMsg);
                return;
            }
            if (result != null)
            {
                if (result.success) MessageBox.Show("添加成功");
                else MessageBox.Show(result.errorMsg);
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void resourceComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    public class Person : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        private long _id;

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


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
