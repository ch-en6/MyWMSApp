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
using wmsApp.pojo;

namespace wmsApp.dialog
{
    public  partial class PermissionDialog : ContentDialog, INotifyPropertyChanged
    {
        public List<Person> People { get; set; }

        long resourceId;

        Dictionary<string, Resource> resources;

        Dictionary<String, long> types;

        List<User> userList;
        public PermissionDialog(Dictionary<string, Resource> resources,long resourceId)
        { 
            this.resources = resources;
            this.resourceId = resourceId;
     
            Result result = PermissionTypesApi.getSelectMap(resourceId);
         
            if (!result.success)
            {
                throw new Exception(result.errorMsg.ToString());
            }

            types = JsonHelper.ConvertToMap<String, long>(result.data.ToString());
            
            InitializeComponent();
 
            initResource();
            initTypes();
            DataContext = this;
        }
    

        private void initResource()
        {
            foreach (KeyValuePair<string, Resource> kvp in resources)
            {
                string key = kvp.Key;
                Resource value = kvp.Value;

                if(value.id == resourceId)
                {
                    resourceComboBox.Text = value.name;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectPersonDialog dialog = new SelectPersonDialog();
            Result result = await dialog.Show();

            People = result.data as List<Person>;

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
            //参数判断
            bool isValid = ValidateFields();
            if (!isValid) return;

            // 遍历 People 集合，检查每个人员的 IsSelected 属性来确定用户选择了哪些人员
            List<long> userIds = new List<long>();
            if (People != null)
            {
               
                foreach (Person person in People)
                {
                    if (person.IsSelected)
                    {
                        // 处理选中的人员
                        userIds.Add(person.Id);

                    }
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

   
}
