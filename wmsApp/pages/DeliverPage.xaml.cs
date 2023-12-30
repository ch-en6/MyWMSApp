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
using wms.utils;
using wmsApp.controls;
using wmsApp.dialog;

namespace wmsApp.pages
{
    /// <summary>
    /// DeliverPage.xaml 的交互逻辑
    /// </summary>
    public partial class DeliverPage : System.Windows.Controls.Page
    {
        public DeliverPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        public async void Add_Click(object sender, RoutedEventArgs e)
        {
            //DeliverDialog dialog = new DeliverDialog();
            //Result typeNameResult = MaterialApi.searchTypeName();
            //List<string> typeNameList = JsonHelper.JsonToList<string>(typeNameResult.data.ToString());
            //dialog.TypeComboBox.ItemsSource = typeNameList;
            //Result userNameResult = UserApi.findAllUserName();
            //List<string> userNameList = JsonHelper.JsonToList<string>(userNameResult.data.ToString());
            //dialog.UserComboBox.ItemsSource = userNameList;
            //ContentDialogResult result = await dialog.ShowAsync();
            NavigationService.Navigate(new Uri("/pages/AddDeliverPage.xaml", UriKind.Relative));
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
