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
using wms.utils;

namespace wmsApp.pages
{
    /// <summary>
    /// MaterialPage.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialPage : Page
    {
        int currentPage = 1;
        long totalPage = 0;

        public MaterialPage()
        {
            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
        }

        public void updatePage()
        {
            Result result = MaterialApi.search(currentPage);

            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                updatePage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                updatePage();

            }
        }
    }
}
