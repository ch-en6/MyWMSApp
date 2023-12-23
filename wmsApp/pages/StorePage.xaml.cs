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
        
        public StorePage()
        {
            Result result = StoreApi.searchAll(currentPage);
            List<Store> storeList = JsonHelper.JsonToList<Store>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = storeList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btnBack_Click(object sender, RoutedEventArgs e)
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

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
