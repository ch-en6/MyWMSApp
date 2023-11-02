using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms;
using wms.dto;
using wms.pojo;
using wms.utils;
using System.Windows.Data;
using CheckBox = System.Windows.Controls.CheckBox;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
using Binding = System.Windows.Data.Binding;
using wms.param;
using System.Data;
using System.Globalization;
using DataGridCell = System.Windows.Controls.DataGridCell;
using System.Collections.ObjectModel;
using ComboBox = System.Windows.Controls.ComboBox;
using wmsApp.param;
using ModernWpf.Controls;
using wmsApp.dialog;
using Button = System.Windows.Controls.Button;

namespace wmsApp.pages

{

    /// <summary>
    /// PermissionPage.xaml 的交互逻辑
    /// </summary>
    public partial class PermissionPage : System.Windows.Controls.Page
    {
        //当前页
        int currentPage = 1;
        //总页数
        int totalPage = 0;
        //页大小
        int pageSize = 4;

        //当前访问的资源id
        long resourceId = 1;

        //表示是否已经加载过当前资源的权限列
        bool flag = true;

        //表示是否已经加载过当前资源的权限列数
        int flag_num = 0;

        //固定人员信息展示列数  id、name、role、操作
        int col_num = 4;

        //所有资源，value为id，key为资源名
        Dictionary<String, String> resources;

        //所有资源类型
        Dictionary<long, List<String>> permissionTypes;

        //所有资源类型
        Dictionary<String, String> resourceTypes;

        //当前显示资源的所有权限类型
        List<String> typeList;

        List<UserPermission> userPermissionList;

        List<UserPermission> usersearchPermissionList;

        //标记是否是搜索 true是  false不是
        bool IsSearching = false;

        //用户id
        long userId = 3;


        private string currentOption;


        public PermissionPage()
            
        {
            //请求返回资源
            Result result = PermissionApi.get_resources();
            if (!result.success)
            {
                MessageBox.Show(result.errorMsg.ToString());
                return;
            }
            resources = JsonHelper.ConvertToMap<String, String>(result.data.ToString());

            //请求返回资源类型
            Result result1 = PermissionApi.get_resourcetypesMap();
            if (!result1.success)
            {
                MessageBox.Show(result1.errorMsg.ToString());
                return;
            }
            resourceTypes = JsonHelper.ConvertToMap<String, String>(result.data.ToString());
          
            //请求返回
            Result result2 = PermissionTypesApi.getPermissionTypesMap();
            if (!result2.success)
            {
                MessageBox.Show(result2.errorMsg.ToString());
                return;
            }
            permissionTypes = JsonHelper.ConvertToMap<long, List<String>>(result2.data.ToString());
           


            InitializeComponent();

            


            dataGrid.Loaded += (sender, e) =>
            {
                setCheckBoxValue();
            };
            
            // 初始化页码

            UpdatePageNumber();

            // 初始化MenuItems
            GenerateMenuItems();

            // 上一页按钮点击事件处理程序
            PreviousPageButton.Click += PreviousPageButton_Click;

            // 下一页按钮点击事件处理程序
            NextPageButton.Click += NextPageButton_Click;

            AddPermissionColumns();


            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            comboBox.SelectedItem = user;


        }


        private void GenerateMenuItems()
        {
            foreach (KeyValuePair<string, string> kvp in resources)
            {
                string key = kvp.Key;
                string value = kvp.Value;

                // 创建新的 NavigationViewItem 对象
                NavigationViewItem newItem = new NavigationViewItem
                {
                    Icon = new SymbolIcon(Symbol.Audio),
                    Content = key,
                    Tag = value
                };

                // 将新的 NavigationViewItem 添加到 MenuItems 集合中
                navigationView.MenuItems.Add(newItem);
            }

            navigationView.ItemInvoked += NavigationView_ItemInvoked;
        }

        

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem selectedItem)
            {
                long selectedResource = long.Parse(selectedItem.Tag.ToString());
                resourceId = selectedResource;
                navigationView.Header = selectedItem.Content;


                currentPage = 1;

                int count = dataGrid.Columns.Count;
                for (int i = col_num; i < count; i++)
                {
                    dataGrid.Columns.RemoveAt(col_num);
                }

                
                AddPermissionColumns();
                UpdatePageNumber();

        }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                currentOption = selectedItem.Name.ToString();
  
                // 在此处处理选项更改后的逻辑
            }
        }


        private void UpdatePageNumber()
        {

            if (!IsSearching)
            {
                // 计算总页数
                totalPage = CalculateTotalPages();
                // 更新当前页数
                Result permissons = PermissionApi.get_permissions(currentPage, resourceId);
                if (permissons == null||!permissons.success)
                {
                    MessageBox.Show(permissons.errorMsg.ToString());
                    userPermissionList = new List<UserPermission>();
                    return;
                }
                userPermissionList = JsonHelper.JsonToList<UserPermission>(permissons.data.ToString());
            }
            else
            {
                string condition = textBox.Text;
                long parsedValue;
                SearchPermissionParams param;
                

                if (long.TryParse(condition, out parsedValue))
                {
                    // 转换成功，可以使用 parsedValue 进行后续操作
                    param = new SearchPermissionParams(
                        parsedValue, condition, condition, resourceId, currentPage);
                }
                else
                {
                    // 转换失败，可以根据需要进行处理
                    param = new SearchPermissionParams(null, condition, condition, resourceId, currentPage);
                }
                Result result;
                Dictionary<String, Object> map;
                List<UserPermission> list;
                switch (currentOption)
                {
                    case "user":
                        result = PermissionApi.searchByUser(param);
                        if (result.data != null)
                        {
                            map = JsonHelper.ConvertToMap<String, Object>(result.data.ToString());
                            list = JsonHelper.JsonToList<UserPermission>(map["records"].ToString());
                            totalPage = int.Parse(map["totalPage"].ToString());
                        }
                        else
                        {
                            list = new List<UserPermission>();
                            totalPage = 1;
                        }
                        usersearchPermissionList = list;
                        break;
                    case "role":
                        result = PermissionApi.searchByRole(param);
                        if (result.data != null)
                        {
                            map = JsonHelper.ConvertToMap<String, Object>(result.data.ToString());
                            list = JsonHelper.JsonToList<UserPermission>(map["records"].ToString());
                            totalPage = int.Parse(map["totalPage"].ToString());
                        }
                        else
                        {
                            list = new List<UserPermission>();
                            totalPage = 1;

                        }
                        usersearchPermissionList = list;
                        //UpdateSearchPageNumber();
                        break;
                }
                
            }


            // 更新页码显示
            PageNumberTextBlock.Text = currentPage.ToString();

            // 更新数据源
            dataGrid.ItemsSource = IsSearching ? usersearchPermissionList : userPermissionList;
         
            // 更新布局
            dataGrid.UpdateLayout();

            // 设置权限列的值
            setCheckBoxValue();

        }



        private int CalculateTotalPages()
        {
            int totalPage = PermissionApi.get_totalpage() | 0;
            return totalPage; // 这里只是示例，你需要根据实际情况修改
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



        public void AddPermissionColumns()
        {
            //请求返回
            Result result2 = PermissionTypesApi.getPermissionTypesMap();
            if (result2.data == null) return;
            permissionTypes = JsonHelper.ConvertToMap<long, List<String>>(result2.data.ToString());

            if (permissionTypes == null || resourceId == null || !permissionTypes.ContainsKey(resourceId))
                return;

            List<string> types = permissionTypes[resourceId];
         
            int count = types.Count;

            for (int i = 0; i < count; i++)
            {
                string permissionName = types[i];

                // 创建一个DataGridTemplateColumn列
                DataGridTemplateColumn column = new DataGridTemplateColumn();
                column.Header = permissionName;
                column.Width = 60; // 设置单元格宽度
                column.IsReadOnly = false; //设置为可修改

                // 创建一个数据模板，包含一个CheckBox
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
                factory.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(OnCheckBoxClick));
                 //factory.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center); // 设置复选框垂直居中
                factory.SetValue(CheckBox.MarginProperty, new Thickness(20, 0, 0, 0)); // 设置内容水平居中

                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = factory;
                column.CellTemplate = cellTemplate;

                // 将列添加到DataGrid中
                dataGrid.Columns.Add(column);
            }

        }

        public void setCheckBoxValue()
        {

            List<UserPermission> list = IsSearching ? usersearchPermissionList : userPermissionList;
            //MessageBox.Show(usersearchPermissionList!=null?usersearchPermissionList.Count.ToString():"无");
            for (int i = 0; i < list.Count; i++)
            {
                var userPermission = list[i];
                List<string> permissionList = userPermission.permissionList;

                for (int j = 0; j < permissionList.Count; j++)
                {
                    string type = permissionList[j];
                    List<string> Types = permissionTypes[resourceId];

                    if (Types.Contains(type))
                    {
                        int columnIndex = GetColumnIndex(type);
                       
                        if (columnIndex >= 0 && columnIndex < dataGrid.Columns.Count)
                        {
                            DataGridTemplateColumn column = (DataGridTemplateColumn)dataGrid.Columns[columnIndex];

                            // 获取列的数据模板
                            DataTemplate cellTemplate = column.CellTemplate;
                            // 在数据模板中查找CheckBox
                            CheckBox checkBox = FindVisualChild<CheckBox>(column.GetCellContent(list[i]) as DependencyObject);

                            if (checkBox != null)
                            {
                                // 设置CheckBox的值
                                checkBox.IsChecked = true; // 或者根据需要设置其他值
                            }
                        }
                    }
                }
            }
        }

        // 查找可视化子元素
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T result)
                {
                    return result;
                }

                T childResult = FindVisualChild<T>(child);

                if (childResult != null)
                {
                    return childResult;
                }
            }

            return null;
        }


   


        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            bool isChecked = checkBox.IsChecked ?? false;

            // 获取 DataGridCell 元素
            DataGridCell cell = FindVisualParent<DataGridCell>(checkBox);

            if (cell != null)
            {
                // 获取 DataGridRow
                DataGridRow row = FindVisualParent<DataGridRow>(cell);

                if (row != null)
                {
                    // 获取行索引
                    int rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(row);

                    // 获取列名
                    string columnName = GetColumnName(cell.Column);
                    // 获取DataGrid中第1列第i行单元格数据
                    var userId = (dataGrid.Columns[0].GetCellContent(dataGrid.Items[rowIndex]) as TextBlock).Text;

                    UpdatePermissionParams param = new UpdatePermissionParams(long.Parse(userId),resourceId,columnName);

                    Result result = PermissionApi.updatePermission(isChecked, param);

                    if (!result.success) MessageBox.Show(result.errorMsg);

                }
            }

            // 处理点击事件逻辑
            // ...
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取点击的按钮
            Button button = (Button)sender;

            // 获取按钮所在的行
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(button.DataContext);

            // 获取行数据
            int rowIndex = row.GetIndex();

            // 处理点击的行数据
            var userId = (dataGrid.Columns[0].GetCellContent(dataGrid.Items[rowIndex]) as TextBlock).Text;

            Result result = PermissionApi.delPermissionByuserId(long.Parse(userId), resourceId);
            if (!result.success) MessageBox.Show(result.errorMsg);
            else MessageBox.Show("已取消");
            UpdatePageNumber();
        }

        // 递归查找指定类型的父级元素
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            if (parent is T parentOfType)
                return parentOfType;
            else
                return FindVisualParent<T>(parent);
        }

        // 获取列名
        private string GetColumnName( DataGridColumn column)
        {
            foreach (DataGridColumn col in dataGrid.Columns)
            {
                if (col == column)
                {
                    // 返回列名
                    return col.Header.ToString();
                }
            }

            return string.Empty;
        }
        /*获取列索引*/
        private int GetColumnIndex(string columnName)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                DataGridColumn column = dataGrid.Columns[i];
                if (column.Header.ToString() == columnName)
                {
                    // 返回列索引
                    return i;
                }
            }

            // 如果找不到匹配的列名，则返回-1表示未找到
            return -1;
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try { 
                PermissionDialog dialog = new PermissionDialog(resources,resourceId);
            
                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Secondary) return;

                int count = dataGrid.Columns.Count;
                for (int i = col_num; i < count; i++)
                {
                    dataGrid.Columns.RemoveAt(col_num);
                }
                AddPermissionColumns();
                UpdatePageNumber();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            try
            {
                DelPermissionTypeDialog dialog = new DelPermissionTypeDialog(resourceId);
                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Secondary) return;

                int count = dataGrid.Columns.Count;
                for (int i = col_num; i < count; i++)
                {
                    dataGrid.Columns.RemoveAt(col_num);
                }
                AddPermissionColumns();
                UpdatePageNumber();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}