using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;

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
                string.IsNullOrEmpty(materialHouseName) ||
                string.IsNullOrEmpty(materialType) ||
                string.IsNullOrEmpty(materialUnit))
            {
                // 设置 args.Cancel 属性为 true，防止用户关闭弹窗
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
                Result AllMaterial = MaterialApi.searchAll();
                List<Material> materials = JsonHelper.JsonToList<Material>(AllMaterial.data.ToString());

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

                bool exists = materials.Any(m => m.name == addMaterial.name && m.houseName == addMaterial.houseName);
                if (exists)
                {
                    args.Cancel = true;
                    MessageBox.Show("该物料在该仓库中已存在!");
                }
                else
                {
                    exists = materials.Any(m => m.name == addMaterial.name);
                    if (exists)
                    {
                        Result equalNameResult = MaterialApi.searchByName(1, addMaterial.name);
                        List<Material> equalNameList = JsonHelper.JsonToList<Material>(equalNameResult.data.ToString());
                        if(addMaterial.type == equalNameList[0].type)
                        {
                            insertMaterial(addMaterial);
                        }
                        else
                        {
                            MessageBoxResult message = MessageBox.Show("在其他仓库中存在同名物料，如果新增该物料，将会同时修改其他仓库中同名物料的类型，是否继续修改？", "提示", MessageBoxButton.YesNo);
                            if (message == MessageBoxResult.Yes)
                            {
                                Result result = MaterialApi.addMaterialEqual(addMaterial);
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
                    }
                    else
                    {
                        insertMaterial(addMaterial);
                    }
                }
            }
        }

        private void insertMaterial(Material material)
        {
            Result result = MaterialApi.addMaterial(material);
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