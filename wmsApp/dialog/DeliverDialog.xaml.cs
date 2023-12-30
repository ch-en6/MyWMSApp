using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Forms;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.pojo;
//using static wmsApp.pages.StorePage;

namespace wmsApp.dialog
{
    /// <summary>
    /// DeliverDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DeliverDialog : ContentDialog
    {
        private List<Product> dataList;
        public List<string> Categories { get; set; }

        List<string> typeMaterialList;

        public DeliverDialog()
        {
            // 其他初始化逻辑
            InitializeComponent();

            dataList = new List<Product>();
            dataList.Add(new Product());
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

        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // 递归查找子元素
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T && child.GetValue(FrameworkElement.NameProperty).ToString() == childName)
                {
                    return (T)child;
                }
                else
                {
                    T result = FindChild<T>(child, childName);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }


        //private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //////int rowIndex = datagrid.Items.IndexOf(datagrid.SelectedCells[0].Item);
        //    //string selectedtype = e.AddedItems[0].ToString();
        //    ////赋值
        //    //var selectedItem = datagrid.SelectedItem as Product;
        //    //selectedItem.type = selectedtype;
        //    //Result typeMaterialResult = MaterialApi.getMaterialNameByType(selectedtype);
        //    //typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
        //    //// 将指定的列表附加到 ComboBox
        //    //NameComboBox.ItemsSource = typeMaterialList;
        //}

        private void NameComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("11");
            var selectedItem = datagrid.SelectedItem as Product;
            string type=selectedItem.type;
            Result typeMaterialResult = MaterialApi.getMaterialNameByType(type);
            typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
            NameComboBox.ItemsSource = typeMaterialList;
        }
        

        //private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // 获取选中的商品类别
        //    string selectedType = (sender as ComboBox).SelectedItem.ToString();

        //    // 获取当前行的DataContext，即数据项
        //    Product currentItem = (datagrid.SelectedItem as Product);

        //    // 更新同一行商品名称列下拉框的数据源
        //    if (currentItem != null)
        //    {
        //        // 获取商品名称列下拉框的单元格
        //        DataGridComboBoxColumn nameComboBoxColumn = datagrid.Columns[1] as DataGridComboBoxColumn;
        //        DataGridCellInfo cellInfo = new DataGridCellInfo(datagrid.SelectedItem, nameComboBoxColumn);
        //        ComboBox nameComboBox = cellInfo.Column.GetCellContent(cellInfo.Item) as ComboBox;
        //        Result typeMaterialResult = MaterialApi.getMaterialNameByType(selectedType);
        //        typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
        //        // 根据选中的商品类别更新商品名称列下拉框的数据源
        //        nameComboBox.ItemsSource = typeMaterialList;

        //        // 清空选择的商品名称，避免选择了不在新数据源中的项
        //        nameComboBox.SelectedIndex = -1;
        //    }
        //}



        //private async void NameLoaded(object sender, EventArgs e)
        //{
        //    //ComboBox comboBox = (ComboBox)sender;
        //    //var selectedItem = datagrid.SelectedItem as Product;
        //    //string selectedtype = selectedItem.type;
        //    //Result typeMaterialResult = MaterialApi.getMaterialNameByType(selectedtype);
        //    //typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
        //    //comboBox.ItemsSource = typeMaterialList;
        //    //comboBox.IsDropDownOpen = true;
        //    //MessageBox.Show(typeMaterialList.Count.ToString());
        //}

       


        //void NameLoaded(object sender, RoutedEventArgs e)
        //{
        //    //获得当前选中项的省份信息
        //    string type = (datagrid.SelectedItem as Product).type;
        //    //查找选中省份下的市作为数据源
        //    Result typeMaterialResult = MaterialApi.getMaterialNameByType(type);
        //    List<string> typeMaterialList = JsonHelper.JsonToList<string>(typeMaterialResult.data.ToString());
        //    ComboBox curComboBox = sender as ComboBox;
        //    //为下拉控件绑定数据源，并选择原选项为默认选项  
        //    string text = curComboBox.Text;
        //    curComboBox.ItemsSource = typeMaterialList;
        //    curComboBox.IsDropDownOpen = true;//获得焦点后下拉

        //}



        //private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
       
        //    var selectedItem = datagrid.SelectedItem as Product;
        //    string type=selectedItem.type.ToString(); 
        //    string selectedName = e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : null;
        //    selectedItem.name = selectedName; 
           
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
