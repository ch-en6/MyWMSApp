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
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wmsApp.pages;

namespace wmsApp.dialog
{
    /// <summary>
    /// UpdateMaterialDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateMaterialDialog : ContentDialog
    {
        public UpdateMaterialDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            long materialId = long.Parse(MaterialIdTextBox.Text);
            string materialName = MaterialNameTextBox.Text;
            int materialStock = int.Parse(MaterialStockTextBox.Text);
            string materialComments = MaterialCommentsTextBox.Text;
            long materialHouseId = long.Parse(MaterialHouseIdComboBox.Text);
            string materialType = (string)MaterialTypeComboBox.Text;
            string materialUnit = (string)MAterialUnitComboBox.Text;
            DateTime materialCreTime = DateTime.Now;

            Material updatedMaterial = new Material()
            {
                id = materialId,
                name = materialName,
                stock = materialStock,
                comments = materialComments,
                houseId = materialHouseId,
                type = materialType,
                unit = materialUnit,
                createTime = materialCreTime
            };

            Result result = MaterialApi.updateMaterial(updatedMaterial);

            if (result.success)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

    }
}
