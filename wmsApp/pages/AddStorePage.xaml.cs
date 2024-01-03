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
using wms.utils;
using wms;
using wmsApp.pojo;
using wms.pojo;
using wmsApp.dialog;

namespace wmsApp.pages
{
    /// <summary>
    /// AddStorePage.xaml 的交互逻辑
    /// </summary>
    public partial class AddStorePage : Page
    {
        private List<IOMaterial> dataList = new List<IOMaterial>();
        List<string> typeMaterialList;

        public AddStorePage()
        {
            InitializeComponent();
            Result result = UserApi.getNowUser();
            User user = JsonHelper.JSONToObject<User>(result.data.ToString());
            UserTextBox.Text = user.id.ToString();
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
                    MessageBox.Show("入库数非数字！");
                    textBox.Text = selectedItem.count.ToString();
                }
            }
        }

        private void NotesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // 获取当前行的数据对象
                //IOMaterial selectedItem = (IOMaterial)datagrid.SelectedItem;
                var selectedItem = datagrid.SelectedItem as IOMaterial;
                // 更新出库数量              
                selectedItem.notes = textBox.Text;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // 创建新窗口实例
            MaterialWindow window = new MaterialWindow(this);

            // 显示新窗口
            window.Show();
        }
        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem is IOMaterial selectedStore)
            {
                dataList.Remove(selectedStore);
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
            NavigationService.Navigate(new Uri("/pages/StorePage.xaml", UriKind.Relative));
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<Store> storeList = new List<Store>();
            // 遍历 DataGrid 中的每一行
            foreach (var item in datagrid.Items)
            {

                // 获取 DataGrid 中每行对应的数据项
                var rowData = item as IOMaterial;

                Result result = UserApi.getNowUser();
                User user = JsonHelper.JSONToObject<User>(result.data.ToString());

                Store store = new Store
                {
                    userId = user.id,
                    materialId = rowData.id,
                    houseName = rowData.houseName,
                    storeCount = rowData.count,
                    notes = rowData.notes,
                };

                if (store.storeCount < 1)
                {
                    MessageBox.Show("入库数必须大于0！");
                    return;
                }
                else
                {
                    storeList.Add(store);
                }
            }
            Result storeResult = StoreApi.storeProcedure(storeList);
            if (storeResult.success)
            {
                MessageBox.Show("入库成功!");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("入库失败");
            }

        }
    }
}
