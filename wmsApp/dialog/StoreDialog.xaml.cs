using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class StoreDialog : ContentDialog
    {
        private List<Store> dataList;
        public List<string> Categories { get; set; }

        public StoreDialog()
        {
            InitializeComponent();
            dataList = new List<Store>();
            
            //datagrid.ItemsSource = dataList;
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
            //dataList.Add(new Deliver());
            //datagrid.ItemsSource = null;
            //datagrid.ItemsSource = dataList;
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            //if (datagrid.SelectedItem is Deliver selectedDeliver)
            //{
            //    dataList.Remove(selectedDeliver);
            //    datagrid.ItemsSource = null;
            //    datagrid.ItemsSource = dataList;
            //}
        }
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Count > 0)
            //{
            //    string selectedCategory = e.AddedItems[0].ToString();

            //    // 确保 MaterialApi 和其方法不为 null
                
            //        Result typeMaterialResult = MaterialApi.typeMaterial(selectedCategory);
            //        // 确保 typeMaterialResult 和其属性不为 null
            //        if (typeMaterialResult != null && typeMaterialResult.data != null)
            //        {
            //            List<string> typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
            //            NameComboBox.ItemsSource = typeMaterialList;
            //        }

            //}
        }



    }
}
