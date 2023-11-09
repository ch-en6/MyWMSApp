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
using wmsApp.dialog;

namespace wmsApp.pages
{
    /// <summary>
    /// PersonnelPage.xaml 的交互逻辑
    /// </summary>
    public partial class PersonnelPage : System.Windows.Controls.Page
    {
        private int currentPage = 1;
        private int itemsPerPage = 2; // 每页显示10条数据
        private long totalPage;
        private int col_num;
        private Dictionary<string, string> resources;
        private long resourceId;

        //标记是否是搜索 true是  false不是
        bool IsSearching = false;
        public PersonnelPage()
        {
            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            Result result = UserApi.getUserMap(currentPage);
            Dictionary<String, Object> map = JsonHelper.ConvertToMap<String, Object>(result.data.ToString());
            List<User> list = JsonHelper.JsonToList<User>(map["records"].ToString());
            totalPage = long.Parse(map["totalPage"].ToString());

            //List<User> list =JsonHelper.JsonToList<User>(result.data.ToString());
            InitializeComponent();
            // 上一页按钮点击事件处理程序
            PreviousPageButton.Click += PreviousPageButton_Click;

            // 下一页按钮点击事件处理程序
            NextPageButton.Click += NextPageButton_Click;
            dataGrid.ItemsSource = list;

        }

        private void UpdatePageNumber()
        {
            PageNumberTextBlock.Text = currentPage.ToString();
            Result result = UserApi.getUserMap(currentPage);
            Dictionary<String, Object> map = JsonHelper.ConvertToMap<String, Object>(result.data.ToString());
            List<User> list = JsonHelper.JsonToList<User>(map["records"].ToString());
            totalPage = long.Parse(map["totalPage"].ToString());
            dataGrid.ItemsSource = list;

        }


        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePageNumber();
            }
        }


        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                UpdatePageNumber();

            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 防止事件继续传递
                e.Handled = true;

                // 触发搜索按钮的点击事件
                Button_Click(sender, e);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsSearching = true;
            UpdatePageNumber();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            IsSearching = false;
            currentPage = 1;
            UpdatePageNumber();
        }
    }
}


