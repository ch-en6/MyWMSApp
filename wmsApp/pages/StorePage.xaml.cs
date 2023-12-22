using NuGet;
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
using wmsApp.pojo;

namespace wmsApp.pages
{
    /// <summary>
    /// StorePage.xaml 的交互逻辑
    /// </summary>
    public partial class StorePage : Page
    {
        int currentPage = 1;
        long totalPage = 0;
        int flag = 0;
        
        public StorePage()
        {
            flag = 0;

            Result result = StoreApi.searchAll(currentPage);
            List<Store> storeList = JsonHelper.JsonToList<Store>(result.data.ToString());
            totalPage = result.total;

            Result houseName = MaterialApi.searchHouseName();
            List<string> houseList = JsonHelper.JsonToList<string>(houseName.data.ToString());

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = storeList;
            warehouseNameComboBox.ItemsSource = houseList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;

            long storeNo = string.IsNullOrEmpty(storeNoTextBox.Text) ? 0 : long.Parse(storeNoTextBox.Text);
            long materialId = string.IsNullOrEmpty(materialIdTextBox.Text) ? 0 : long.Parse(materialIdTextBox.Text);
            string warehouseName = warehouseNameComboBox.Text;
            DateTime? startTime = startTimeTextBox.SelectedDate;
            DateTime? endTime = endTimeTextBox.SelectedDate;
            long operatorId = string.IsNullOrEmpty(operatorIdTextBox.Text) ? 0 : long.Parse(operatorIdTextBox.Text);
            string notes = storeNotes.Text;
            currentPage = 1;

            if(startTime.HasValue && endTime.HasValue && endTime < startTime)
            {
                MessageBox.Show("结束时间不能早于开始时间，请重新选择");
                return;
            }

            Result result = StoreApi.searchCondition(storeNo, warehouseName, startTime, endTime, materialId, operatorId, notes, currentPage);
            List<Store> storeList = JsonHelper.JsonToList<Store>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = storeList;
        }

        public void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Result result = StoreApi.searchAll(currentPage);
            List<Store> storeList = JsonHelper.JsonToList<Store>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = storeList;
        }

        public void btnClear_Click(object sender, RoutedEventArgs e)
        {
            storeNoTextBox.Text = "";
            materialIdTextBox.Text = "";
            warehouseNameComboBox.SelectedIndex = -1;
            startTimeTextBox.SelectedDate = null;
            endTimeTextBox.SelectedDate = null;
            operatorIdTextBox.Text = "";
            storeNotes.Text = "";
        }

        public void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        public void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                if(flag == 0)
                {
                    Result result = StoreApi.searchAll(currentPage);
                    List<Store> storeList = JsonHelper.JsonToList<Store>(result.data.ToString());
                    totalPage = result.total;

                    PageNumberTextBlock.Text = totalPage.ToString();
                    datagrid.ItemsSource = storeList;
                }
            }

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                
            }
        }

    }
}
