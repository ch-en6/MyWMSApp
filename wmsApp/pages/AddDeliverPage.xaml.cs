using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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

                // 创建一个 Deliver 对象并将数据添加到 List<Deliver> 中
                Deliver deliver = new Deliver
                {
                    materialId = rowData.id,
                    houseName = rowData.houseName,
                    deliverCount = rowData.count,
                    notes = rowData.notes,
                };

                // 将 Deliver 对象添加到 List<Deliver> 中
                deliverList.Add(deliver);
                Result result=DeliverApi.multiDelivery(deliverList);
                if(result.success)
                {
                    MessageBox.Show("入库成功!");
                }
                else
                {
                    MessageBox.Show("库存不足，入库失败");
                }
            }
        }
    }
}
