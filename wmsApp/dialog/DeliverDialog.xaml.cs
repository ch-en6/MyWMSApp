using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.pojo;

namespace wmsApp.dialog
{
    /// <summary>
    /// DeliverDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DeliverDialog : ContentDialog
    {
        private List<Product> dataList;
        public List<string> Categories { get; set; }

        public DeliverDialog()
        {
            // 其他初始化逻辑
            InitializeComponent();

            dataList = new List<Product>();
            datagrid.ItemsSource = dataList;
            //Result typeNameResult = MaterialApi.searchTypeName();
            //Categories = JsonHelper.JsonToList<string>(typeNameResult.data.ToString());
           

            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void datagrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            dataList.Add(new Product());
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = dataList;
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem is Product selectedDeliver)
            {
                dataList.Remove(selectedDeliver);
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = dataList;
            }
        }
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedtype = e.AddedItems[0].ToString();
            //赋值
            var selectedItem = datagrid.SelectedItem as Product;
            selectedItem.type = selectedtype;
            Result typeMaterialResult = MaterialApi.getMaterialNameByType(selectedtype);
            List<string> typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
            NameComboBox.ItemsSource = typeMaterialList;
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = datagrid.SelectedItem as Product;
            string type=selectedItem.type.ToString(); 
            string selectedName = e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : null;
            selectedItem.name = selectedName; 
           
            // 使用选中的类别和名称调用 API 获取结果
            Result reslut = MaterialApi.getMaterialNameByType(type);

            // 将结果转换为 List<string>
            List<string> list = JsonHelper.JsonToList<string>(reslut.data.ToString());
            HouseComboBox.ItemsSource = list;
        }
        private void HouseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = datagrid.SelectedItem as Product;
            string name = selectedItem. name.ToString();
            string selectedHouse = e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : null;
            selectedItem.house = selectedHouse;
            Result reslut = MaterialApi.getMaterialByNameAndHouse(name, selectedHouse);
            Material material= JsonHelper.JSONToObject<Material>(reslut.data.ToString());
            selectedItem.materialId = material.id;
            selectedItem.stock = material.stock;
        }
        
    }
}
