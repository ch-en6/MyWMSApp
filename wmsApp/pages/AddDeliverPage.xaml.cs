using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.dialog;
using wmsApp.pojo;

namespace wmsApp.pages
{
    /// <summary>
    /// DeliverPage.xaml 的交互逻辑
    /// </summary>
    public partial class AddDeliverPage : System.Windows.Controls.Page
    {
        private List<IOMaterial> dataList;

        List<string> typeMaterialList;
        public AddDeliverPage()
        {
            InitializeComponent();
            Result result = UserApi.getNowUser();
            wms.pojo.User user = JsonHelper.JSONToObject<wms.pojo.User>(result.data.ToString());
            UserTextBox.Text = user.id.ToString();
            dataList = new List<IOMaterial>();
        }

        private void datagrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            // 创建新窗口实例
            MaterialWindow window = new MaterialWindow(this);

            // 显示新窗口
            window.Show();
        }
        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem is IOMaterial selectedDeliver)
            {
                dataList.Remove(selectedDeliver);
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = dataList;
            }
        }
        public void AddNewRow(IOMaterial ioMaterial)
        {
            dataList.Add(ioMaterial);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = dataList;
        }
        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/pages/DeliverPage.xaml", UriKind.Relative));
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<Deliver> deliverList = new List<Deliver>();
            // 遍历 DataGrid 中的每一行
            foreach (var item in datagrid.Items)
            {
                // 获取 DataGrid 中每行对应的数据项
                var rowData = item as IOMaterial; // 请替换成你实际的数据类型

                Result result = UserApi.getNowUser();
                wms.pojo.User user = JsonHelper.JSONToObject<wms.pojo.User>(result.data.ToString());

                // 创建一个 Deliver 对象并将数据添加到 List<Deliver> 中
                Deliver deliver = new Deliver
                {
                    userId = user.id,
                    materialId = rowData.id,
                    houseName = rowData.houseName,
                    deliverCount = rowData.count,
                    notes = rowData.notes,
                };

                if (deliver.deliverCount < 1)
                {
                    MessageBox.Show("入库数必须大于0！");
                    break;
                }
                else
                {
                    // 将 Deliver 对象添加到 List<Deliver> 中
                    deliverList.Add(deliver);
                }
            }
            Result deliverResult = DeliverApi.multiDelivery(deliverList);
            if (deliverResult.success)
            {
                MessageBox.Show("出库成功!");
                NavigationService.Navigate(new Uri("/pages/DeliverPage.xaml", UriKind.Relative));

            }
            else
            {
                MessageBox.Show("库存不足，出库失败");
            }
        }

        private void CountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // 获取当前行的数据对象
                //IOMaterial selectedItem = (IOMaterial)datagrid.SelectedItem;
                var selectedItem = datagrid.SelectedItem as IOMaterial;
                // 更新出库数量
                if (int.TryParse(textBox.Text, out int newCount))
                {
                    selectedItem.count = newCount;
                }
                else
                {
                    // 处理无效输入，比如非数字
                    MessageBox.Show("出库数非数字！");
                    textBox.Text = selectedItem.count.ToString();
                }
            }
        }

    }
}
