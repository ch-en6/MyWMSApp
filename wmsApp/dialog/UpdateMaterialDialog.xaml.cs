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
            int materialStock;
            bool success = int.TryParse(MaterialStockTextBox.Text, out materialStock);
            string materialComments = MaterialCommentsTextBox.Text;
            long materialHouseId;
            success &= long.TryParse(MaterialHouseIdComboBox.Text, out materialHouseId);
            string materialType = (string)MaterialTypeComboBox.Text;
            string materialUnit = (string)MAterialUnitComboBox.Text;

            if(string.IsNullOrEmpty(materialName) ||
                !success ||
                string.IsNullOrEmpty(materialType) ||
                string.IsNullOrEmpty(materialUnit))
            {
                args.Cancel = true;
                MessageBox.Show("请填写完整的物料信息");
            }
            else if(materialStock < 0)
            {
                args.Cancel = true;
                MessageBox.Show("请填写正确库存");
            }
            else
            {
                Material updatedMaterial = new Material()
                {
                    id = materialId,
                    name = materialName,
                    stock = materialStock,
                    comments = materialComments,
                    houseId = materialHouseId,
                    type = materialType,
                    unit = materialUnit,
                    createTime = DateTime.Now
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
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

    }
}
