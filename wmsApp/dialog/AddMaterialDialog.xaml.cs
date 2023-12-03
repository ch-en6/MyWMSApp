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

namespace wmsApp.dialog
{
    /// <summary>
    /// AddMaterialDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddMaterialDialog : ContentDialog
    {
        public AddMaterialDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string materialName = MaterialNameTextBox.Text;
            int materialStock = int.Parse(MaterialStockTextBox.Text);
            string materialComments = MaterialCommentsTextBox.Text;
            long materialHouseId = long.Parse(MaterialHouseIdComboBox.Text);
            string materialType = (string)MaterialTypeComboBox.Text;
            string materialUnit = (string)MAterialUnitComboBox.Text;

            Material addMaterial = new Material()
            {
                name = materialName,
                stock = materialStock,
                comments = materialComments,
                houseId = materialHouseId,
                type = materialType,
                unit = materialUnit,
                createTime = DateTime.Now
            };

            Result result = MaterialApi.addMaterial(addMaterial);

            if (result.success)
            {
                MessageBox.Show("新增成功");
            }
            else
            {
                MessageBox.Show("新增失败");
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

    }

}