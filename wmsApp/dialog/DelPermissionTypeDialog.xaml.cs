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
using wms.utils;
using wmsApp.controls;

namespace wmsApp.dialog
{
    /// <summary>
    /// DelPermissionTypeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DelPermissionTypeDialog : ContentDialog
    {
        List<String> typeList;
        long resourceId;
        public DelPermissionTypeDialog(long resourceId)
        { 
            Result result = PermissionApi.get_resource_types(resourceId);
            if (!result.success)
            {
                throw new Exception(result.errorMsg.ToString());
            }
            if (result.data != null)
            {
                typeList = JsonHelper.JsonToList<String>(result.data.ToString());
            }

            InitializeComponent();

            this.resourceId = resourceId;

           
            combox.ItemsSource = typeList;
        }
        private bool ValidateComboBoxNotEmpty()
        {
            if (combox.SelectedItem != null)
            {
                // ComboBox 不为空
                return true;
            }
            else
            {
                // ComboBox 为空
                MessageBox.Show("选项不能为空");
                return false;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool isValid = ValidateComboBoxNotEmpty();
            if (!isValid) return;
            string type = combox.SelectedItem.ToString();

            Result result = PermissionTypesApi.delPermissionType(resourceId, type);
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg.ToString());
                return;
            }
            if (result != null && result.success) MessageBox.Show("删除成功!");
            else MessageBox.Show(result.errorMsg);
        }
    }
}
