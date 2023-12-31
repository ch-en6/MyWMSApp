using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Management;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wmsApp.pojo;
using wms.utils;
using wmsApp.pages;
using wms.pojo;

namespace wmsApp.dialog
{
    /// <summary>
    /// UpdateMaterialDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateMaterialDialog : ContentDialog
    {
        private string previousMaterialName;
        private string previousMaterialType;
        public UpdateMaterialDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            previousMaterialName = MaterialNameTextBox.Text;
            previousMaterialType = MaterialTypeComboBox.SelectedItem.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            long materialId = long.Parse(MaterialIdTextBox.Text);
            string materialName = MaterialNameTextBox.Text;
            int materialStock;
            bool success = int.TryParse(MaterialStockTextBox.Text, out materialStock);
            string materialComments = MaterialCommentsTextBox.Text;
            string materialHouseName = MaterialHouseNameTextBox.Text;
            string materialType = MaterialTypeComboBox.Text;
            string materialUnit = MAterialUnitComboBox.Text;

            if(string.IsNullOrEmpty(materialName) ||
                string.IsNullOrEmpty(materialHouseName) ||
                string.IsNullOrEmpty(materialType) ||
                string.IsNullOrEmpty(materialUnit))
            {
                args.Cancel = true;
                MessageBox.Show("请填写完整的物料信息");
            }
            else if(materialStock < 0 || !success)
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
                    houseName = materialHouseName,
                    type = materialType,
                    unit = materialUnit,
                    createTime = DateTime.Now
                };

                Result AllMaterial = MaterialApi.searchAll();
                List<Material> materials = JsonHelper.JsonToList<Material>(AllMaterial.data.ToString());
                
                if(materialName != previousMaterialName)
                {
                    bool exits = materials.Any(m => m.name == materialName && m.houseName == materialHouseName);
                    if (exits)
                    {
                        args.Cancel = true;
                        MessageBox.Show("该物料在该仓库已存在!");
                    }
                    else
                    {
                        judgeTypeIsEqual(materials, updatedMaterial, args);
                    }
                }
                else
                {
                    judgeTypeIsEqual(materials, updatedMaterial, args);
                }
            }
        }

        private void modifyMaterial(Material material)
        {
            Result result = MaterialApi.updateMaterial(material);
            if (result.success)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
        }

        private void judgeTypeIsEqual(List<Material> materials, Material updateMaterial, ContentDialogButtonClickEventArgs args)
        {
            if (updateMaterial.type != previousMaterialType)
            {
                bool exits = materials.Any(m => m.name == updateMaterial.name && m.houseName != updateMaterial.houseName);
                if (exits)
                {
                    MessageBoxResult message = MessageBox.Show("修改该物料类型，将会同时修改其他仓库中同名物料的类型，是否继续修改？", "提示", MessageBoxButton.YesNo);
                    if (message == MessageBoxResult.Yes)
                    {
                        Result result = MaterialApi.updateEqualType(updateMaterial);
                        if (result.success)
                        {
                            MessageBox.Show("更新成功");
                        }
                        else
                        {
                            MessageBox.Show("更新失败");
                        }
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                }
                else
                {
                    modifyMaterial(updateMaterial);
                }
            }
            else
            {
                modifyMaterial(updateMaterial);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

    }
}
