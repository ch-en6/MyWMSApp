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
using wmsApp.controls;
using wmsApp.utils;
using wmsApp.pojo;
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

        //当前访问的资源id
        long resourceId = 1;

        //表示是否已经加载过当前资源的权限列
        bool flag = true;

        //表示是否已经加载过当前资源的权限列数
        int flag_num = 0;

        //固定人员信息展示列数  id、name、role、操作
        int col_num = 4;

        //所有资源，资源名，Resource
        private Dictionary<String, Resource> resources;

        //所有资源对应权限类型map resourceId ，所有权限类型
        private Dictionary<long, List<String>> permissionTypes;
/*
        //所有权限类型 typeId ，
        private Dictionary<String, String> resourceTypes;*/

        //当前显示资源的所有权限类型
        private List<String> typeList;

        //所有用户权限信息
        private List<UserPermission> userPermissionList;
        //所有搜索的用户权限信息
        private List<UserPermission> usersearchPermissionList;

        //标记是否是搜索 true是  false不是
        private bool IsSearching = false;

        private string currentOption;


        public PermissionPage()

        {
            try
            {
                //请求返回资源
                Result result = ResourceApi.getResources();

                if (!result.success)
                {
                    if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                    ModernMessageBox.showMessage(result.errorMsg.ToString());
                    return;
                }

                resources = JsonHelper.ConvertToMap<String, Resource>(result.data.ToString());
              
/*
                //请求返回资源类型
                Result result1 = PermissionApi.get_resourcetypesMap();
                if (!result1.success)
                {
                    if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                    ModernMessageBox.showMessage(result.errorMsg.ToString());
                    return;
                }
                resourceTypes = JsonHelper.ConvertToMap<String, String>(result.data.ToString());*/

                //请求返回资源所对应的权限类型
                Result result2 = PermissionTypesApi.getPermissionTypesMap();
                if (!result2.success)
                {
                    if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                    ModernMessageBox.showMessage(result.errorMsg.ToString());
                    return;
                }
                permissionTypes = JsonHelper.ConvertToMap<long, List<String>>(result2.data.ToString());
            }
            catch (TokenExpiredException ex)
            {

            };


            InitializeComponent();


            // 初始化表格

            dataGrid.Loaded += (sender, e) =>
            {
                setCheckBoxValue();
            };

            UpdatePageNumber();

            // 初始化MenuItems
            GenerateMenuItems();

            // 添加当前资源的权限类型列
            AddPermissionColumns();


            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            comboBox.SelectedItem = user;


        }

        private Symbol GetIconFromName(string iconName)
        {
            switch (iconName)
            {
                case "People":
                    return Symbol.People;
                case "Home":
                    return Symbol.Home;
                case "Download":
                    return Symbol.Download;
                case "Upload":
                    return Symbol.Upload;
                case "ProtectedDocument":
                    return Symbol.ProtectedDocument;
                case "Contact":
                    return Symbol.Contact;
                default:
                    return Symbol.Download;
            }
        }

        private void GenerateMenuItems()
        {
            // 遍历资源
            foreach (KeyValuePair<string, Resource> kvp in resources)
            {
                string key = kvp.Key;
                Resource value = kvp.Value;
                //获取图标对象
                Symbol iconSymbol = GetIconFromName(value.icon);
                // 创建新的 NavigationViewItem 对象
                NavigationViewItem newItem = new NavigationViewItem
                {
                    Icon = new SymbolIcon(iconSymbol),
                    Content = value.name,
                    Tag = value.id,
                    FontSize = 22,
                    Height = 50
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
                //获取到点击的资源的id
                long selectedResource = long.Parse(selectedItem.Tag.ToString());
                resourceId = selectedResource;
                //选中导航项
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
            }
        }


        private void UpdatePageNumber()
        {
            try
            {
                if (!IsSearching)
                {
                    // 计算总页数
                    totalPage = CalculateTotalPages();
                    // 更新当前页数
                    Result permissons = PermissionApi.get_permissions(currentPage, resourceId);
                    if (permissons.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                    if (permissons == null || !permissons.success)
                    {
                        ModernMessageBox.showMessage(permissons.errorMsg.ToString());
                        userPermissionList = new List<UserPermission>();
                        return;
                    }
                    userPermissionList = JsonHelper.JsonToList<UserPermission>(permissons.data.ToString());
                }
                else
                {
                    string condition = textBox.Text;
                    if (condition == "")
                    {
                        ModernMessageBox.showMessage("参数不能为空!!");
                        IsSearching = !IsSearching;
                        return;
                    }
                    long parsedValue;
                    SearchPermissionParams param;


                    if (long.TryParse(condition, out parsedValue))
                    {
                        // 转换成功，可以使用 parsedValue 进行后续操作
                        param = new SearchPermissionParams(parsedValue, condition, condition, resourceId, currentPage);
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
                            if (result==null)
                            {
                                ModernMessageBox.showMessage("无访问权限");
                                return;
                            }
                            if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                            
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
                            if (result == null)
                            {
                                ModernMessageBox.showMessage("无访问权限");
                                return;
                            }
                            if (result == null) ModernMessageBox.showMessage("无访问权限");
                            if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
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
                PageNumberTextBlock.Text = currentPage.ToString()+"/"+totalPage.ToString();

                // 更新数据源
                dataGrid.ItemsSource = IsSearching ? usersearchPermissionList : userPermissionList;

                // 更新布局
                dataGrid.UpdateLayout();

                // 设置权限列的值
                setCheckBoxValue();
            }
            catch (TokenExpiredException) { };

        }



        private int CalculateTotalPages()
        {
            int totalPage = PermissionApi.get_totalpage() | 1;
            return totalPage; 
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
            try
            {
             /*   //请求返回当前资源对应的所有权限类型
                Result result2 = PermissionTypesApi.getPermissionTypesMap();
                if (result2.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                if (result2.data == null) return;
                permissionTypes = JsonHelper.ConvertToMap<long, List<String>>(result2.data.ToString());

                if (permissionTypes == null || resourceId == null || !permissionTypes.ContainsKey(resourceId))
                    return;*/
                
                //获取当前资源的权限类型列表
                List<string> types = permissionTypes[resourceId];

                int count = types.Count;

                //遍历权限类型列表
                for (int i = 0; i < count; i++)
                {
                    string permissionName = types[i];

                    // 创建一个DataGridTemplateColumn列
                    DataGridTemplateColumn column = new DataGridTemplateColumn();
                    column.Header = permissionName;
                    column.Width = DataGridLength.SizeToCells; // 设置单元格宽度
                    column.IsReadOnly = false; //设置为可修改
                    

                    // 创建一个数据模板，包含一个CheckBox
                    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
                    factory.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(OnCheckBoxClick));
                    factory.SetValue(CheckBox.MarginProperty, new Thickness(20, 0, 0, 0)); // 设置内容水平居中

                    DataTemplate cellTemplate = new DataTemplate();
                    cellTemplate.VisualTree = factory;
                    column.CellTemplate = cellTemplate;


                    // 将列添加到DataGrid中
                    dataGrid.Columns.Add(column);
                }
            }
            catch (TokenExpiredException ex) { }

        }

        public void setCheckBoxValue()
        {
            //判断现在如果是全部 ，就取userPermissionList，如果是查询就取usersearchPermissionList
            List<UserPermission> list = IsSearching ? usersearchPermissionList : userPermissionList;

            for (int i = 0; i < list.Count; i++)
            {
                //获取一行用户权限数据
                var userPermission = list[i];
                //获取该用户的权限有哪些
                List<string> permissionList = userPermission.permissionList;
                //遍历用户的权限
                for (int j = 0; j < permissionList.Count; j++)
                {
                    //获得权限名称
                    string type = permissionList[j];
                    //获取当前选中资源的权限类型列表
                    List<string> Types = permissionTypes[resourceId];
                    //用户是否有这个权限
                    if (Types.Contains(type))
                    {
                        //有的话就获取行号列号
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

        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            //获取到当前点击的checkbox
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
                    // 获取行号
                    int rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(row);

                    // 获取列名
                    string columnName = GetColumnName(cell.Column);

                    // 获取第一列该行的用户id
                    var userId = (dataGrid.Columns[0].GetCellContent(dataGrid.Items[rowIndex]) as TextBlock).Text;

                    UpdatePermissionParams param = new UpdatePermissionParams(long.Parse(userId), resourceId, columnName,isChecked);
                    try
                    {
                        Result result = PermissionApi.updatePermission(param);
                        if (result.code == Constants.TOKEN_ILLEGAL_EXIST) throw new TokenExpiredException();
                        if (!result.success)
                        {
                            checkBox.IsChecked = !isChecked;
                            ModernMessageBox.showMessage(result.errorMsg);

                        }
                    }
                    catch (TokenExpiredException ex) {
                    }

                }
            }

        
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取点击的按钮
            Button button = (Button)sender;

            // 获取按钮所在的行
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(button.DataContext);

            // 获取行数据
            int rowIndex = row.GetIndex();

            // 处理点击的行的用户的id
            var userId = (dataGrid.Columns[0].GetCellContent(dataGrid.Items[rowIndex]) as TextBlock).Text;

            ContentDialog dialog = new ContentDialog()
            {
                Title = "提示",
                Content = "确认取消吗？",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            ContentDialogResult select = await dialog.ShowAsync();
     
            if (select == ContentDialogResult.Secondary) return;

            Result result = PermissionApi.updatePermissionByuserId(long.Parse(userId), resourceId, false);

            if (!result.success) ModernMessageBox.showMessage(result.errorMsg);
            else ModernMessageBox.showMessage("已取消");
            UpdatePageNumber();
        }

        private async void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取点击的按钮
            Button button = (Button)sender;

            // 获取按钮所在的行
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(button.DataContext);

            // 获取行数据
            int rowIndex = row.GetIndex();

            // 处理点击的行的用户id
            var userId = (dataGrid.Columns[0].GetCellContent(dataGrid.Items[rowIndex]) as TextBlock).Text;

            ContentDialog dialog = new ContentDialog()
            {
                Title = "提示",
                Content = "确认全选吗？",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            ContentDialogResult select = await dialog.ShowAsync();
            if (select == ContentDialogResult.Secondary) return;

            Result result = PermissionApi.updatePermissionByuserId(long.Parse(userId), resourceId, true);
            if (!result.success) ModernMessageBox.showMessage(result.errorMsg);
            else ModernMessageBox.showMessage("已全选");
            UpdatePageNumber();
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
            currentPage = 1;
            UpdatePageNumber();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            IsSearching = false;
            currentPage = 1;
            UpdatePageNumber();
        }

  


        private async void AddPermissionType(object sender, RoutedEventArgs e)
        {
            try
            {
                PermissionDialog dialog = new PermissionDialog(resources, resourceId);

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
            catch (Exception ex)
            {
                ModernMessageBox.showMessage(ex.Message);
            }
        }

        private async void DelPermissionType(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {
                ModernMessageBox.showMessage(ex.Message);
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
        private string GetColumnName(DataGridColumn column)
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
    }
}