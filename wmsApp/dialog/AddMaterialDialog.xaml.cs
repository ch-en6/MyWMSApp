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
            int materialStock;
            bool success = int.TryParse(MaterialStockTextBox.Text, out materialStock);
            string materialComments = MaterialCommentsTextBox.Text;
            string materialHouseName = (string)MaterialHouseNameComboBox.Text;
            string materialType = (string)MaterialTypeComboBox.Text;
            string materialUnit = (string)MAterialUnitComboBox.Text;

            if (string.IsNullOrEmpty(materialName) ||
                !success ||
                string.IsNullOrEmpty(materialHouseName) ||
                string.IsNullOrEmpty(materialType) ||
                string.IsNullOrEmpty(materialUnit))
            {
                // 设置 args.Cancel 属性为 true，防止用户关闭弹窗
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
                // 执行添加物料操作
                Material addMaterial = new Material()
                {
                    name = materialName,
                    stock = materialStock,
                    comments = materialComments,
                    houseName = materialHouseName,
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
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

    }

}