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

namespace wmsApp.pages
{
    /// <summary>
    /// HouseManagementPage.xaml 的交互逻辑
    /// </summary>
    public partial class HouseManagementPage : Page
    {
        int currentPage = 1;
        long totalPage = 0;
        string pageNumText;

        public HouseManagementPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }



        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
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
