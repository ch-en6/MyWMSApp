using LiveCharts.Helpers;
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
    /// UserPage.xaml 的交互逻辑
    /// </summary>
    public partial class InStatisticPage : System.Windows.Controls.Page
    {
        int flag = 0;
        DateTime? startTime;
        DateTime? endTime;
        public InStatisticPage()
        {
            InitializeComponent();

            // 设置结束日期为今天
            endTimeTextBox.SelectedDate = DateTime.Today;
            endTime = endTimeTextBox.SelectedDate;
            // 设置开始日期为今天往前的30天
            startTimeTextBox.SelectedDate = DateTime.Today.AddDays(-30);
            startTime = startTimeTextBox.SelectedDate;
            // 初始化图表数据
            StoreUpdateChartData(startTime, endTime);
        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // 处理开始和结束日期的更改
            DateTime? startTime = startTimeTextBox.SelectedDate;
            DateTime? endTime = endTimeTextBox.SelectedDate;
            if (startTime.HasValue && endTime.HasValue && endTime < startTime)
            {
                MessageBox.Show("结束时间不能早于开始时间，请重新选择");
                return;
            }
            if(flag==0)
            {
                StoreUpdateChartData(startTime, endTime);
            }
            else
            {
                DeliverUpdateChartData(startTime, endTime);
            }
            // 执行数据检索和初始化
            
        }

        private void StoreUpdateChartData(DateTime? startTime, DateTime? endTime)
        {
            Result NameResult = StoreApi.findNameBetweenDates(startTime, endTime);
            List<string> NameList = JsonHelper.JsonToList<string>(NameResult.data.ToString());

            Result CountResult = StoreApi.findCountByNameBetweenDates(startTime, endTime);
            List<int> CountList = JsonHelper.JsonToList<int>(CountResult.data.ToString());
            // 创建 StatisticData 对象并设置值
            LvcView data = new LvcView();
            data.ChartDatas = CountList.AsChartValues();
            data.XLables = NameList;

            // 设置 DataContext
            this.DataContext = data;
        }

        private void DeliverUpdateChartData(DateTime? startTime, DateTime? endTime)
        {
            Result NameResult = DeliverApi.findNameBetweenDates(startTime, endTime);
            List<string> NameList = JsonHelper.JsonToList<string>(NameResult.data.ToString());

            Result CountResult = DeliverApi.findCountByNameBetweenDates(startTime, endTime);
            List<int> CountList = JsonHelper.JsonToList<int>(CountResult.data.ToString());
            // 创建 StatisticData 对象并设置值
            LvcView data = new LvcView();
            data.ChartDatas = CountList.AsChartValues();
            data.XLables = NameList;

            // 设置 DataContext
            this.DataContext = data;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = TypeComboBox.SelectedItem as ComboBoxItem;
            string selectedQueryType = selectedComboBoxItem.Name;
            Result result = null;
            List<User> userList;

            switch (selectedQueryType)
            {
                case "store":
                    flag = 0;
                    StoreUpdateChartData(startTime, endTime);
                    break;
                case "deliver":
                    DeliverUpdateChartData(startTime, endTime);
                    break;

            }
        }


        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {
            // 在这里可以对图表进行其他初始化操作
        }
    }
}
