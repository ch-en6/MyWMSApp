using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.dialog;
using wmsApp.pojo;

namespace wmsApp.pages
{
    public partial class MaterialPage : System.Windows.Controls.Page
    {
        int currentPage = 1;
        long totalPage = 0;
        int flag = 0;
        string pageNumText;

        public MaterialPage()
        {
            flag = 0;
            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            pageNumText = currentPage.ToString() + "/" + totalPage.ToString();
            PageNumberTextBlock.Text = pageNumText;
            datagrid.ItemsSource = materialList;
        }

        public void updatePage()
        {
            Result result;
            List<Material> materialList = null;

            switch (flag)
            {
                case 0:
                    result = MaterialApi.search(currentPage);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
                case 1:
                    result = searchId(textBox.Text);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
                case 2:
                    result = searchName(textBox.Text);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
                case 3:
                    result = searchHouseName(textBox.Text);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
                case 4:
                    result = searchType(textBox.Text);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
                case 5:
                    result = searchComments(textBox.Text);
                    materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
                    break;
            }
            pageNumText = currentPage.ToString() + "/" + totalPage.ToString();
            PageNumberTextBlock.Text = pageNumText;
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

        private Result searchId(string userInput)
        {
            long id;
            if (userInput == "")
            {
                id = -1;
            }
            else
            {
                long.TryParse(userInput, out id);
            }
            Result result = MaterialApi.searchById(currentPage, id);
            return result;
        }

        private Result searchName(string userInput)
        {
            if (userInput == "")
            {
                userInput = "...";
            }
            Result result = MaterialApi.searchByName(currentPage, userInput);
            return result;
        }

        private Result searchHouseName(string userInput)
        {
            if (userInput == "")
            {
                userInput = "...";
            }
            Result result = MaterialApi.searchByHouseName(currentPage, userInput);
            return result;
        }

        private Result searchType(string userInput)
        {
            if (userInput == "")
            {
                userInput = "...";
            }
            Result result = MaterialApi.searchByType(currentPage, userInput);
            return result;
        }

        private Result searchComments(string userInput)
        {
            if (userInput == "")
            {
                userInput = ".nothing.";
            }
            Result result = MaterialApi.searchByComments(currentPage, userInput);
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
            string selectedQueryType = selectedComboBoxItem.Name;
            string userInput = textBox.Text;
            currentPage = 1;
            Result result = null;
            List<Material> materialList;

            switch (selectedQueryType)
            {
                case "materialID":
                    flag = 1;
                    result = searchId(userInput);
                    break;
                case "meterialName":
                    flag = 2;
                    result = searchName(userInput);
                    break;
                case "meterialHouseName":
                    flag = 3;
                    result = searchHouseName(userInput);
                    break;
                case "meterialType":
                    flag = 4;
                    result = searchType(userInput);
                    break;
                case "meterialComments":
                    flag = 5;
                    result = searchComments(userInput);
                    break;
            }
            materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            pageNumText = currentPage.ToString() + "/" + totalPage.ToString();
            PageNumberTextBlock.Text = pageNumText;
            datagrid.ItemsSource = materialList;
        }


        private async void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = datagrid.SelectedItem as Material;

            Result typeNameResult = MaterialApi.searchTypeName();
            List<string> typeNameList = JsonHelper.JsonToList<string>(typeNameResult.data.ToString());

            // 将数据分配给对应的TextBox或ComboBox
            UpdateMaterialDialog dialog = new UpdateMaterialDialog();
            dialog.MaterialIdTextBox.Text = selectedItem.id.ToString();
            dialog.MaterialNameTextBox.Text = selectedItem.name;
            dialog.MaterialStockTextBox.Text = selectedItem.stock.ToString();
            dialog.MaterialCommentsTextBox.Text = selectedItem.comments;
            dialog.MaterialHouseNameTextBox.Text = selectedItem.houseName.ToString();

            dialog.MaterialTypeComboBox.ItemsSource = typeNameList;
            dialog.MaterialTypeComboBox.SelectedValue = selectedItem.type.ToString();

            dialog.MAterialUnitComboBox.Text = selectedItem.unit;

            ContentDialogResult dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Secondary) return;

            updatePage();

        }

        private void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = datagrid.SelectedItem as Material;

            if (selectedItem.stock > 0)
            {
                MessageBox.Show("该物料库存大于0，不允许删除！");
            }
            else if (MaterialApi.ifStoreOrDeliver(selectedItem.id).success)
            {
                MessageBox.Show("该物料有出库或入库记录，不允许删除！");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Result result1 = MaterialApi.deleteMaterial(selectedItem.id);
                    if (result1.success)
                    {
                        MessageBox.Show("删除成功");
                        datagrid.Items.Refresh();
                        updatePage();
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }

            }
        }

        private async void AddMaterialButton_Click_1(object sender, RoutedEventArgs e)
        {
            Result houseNameResult = MaterialApi.searchHouseName();
            List<string> houseList = JsonHelper.JsonToList<string>(houseNameResult.data.ToString());
            Result typeNameResult = MaterialApi.searchTypeName();
            List<string> typeNameList = JsonHelper.JsonToList<string>(typeNameResult.data.ToString());

            AddMaterialDialog dialog = new AddMaterialDialog();

            dialog.MaterialHouseNameComboBox.ItemsSource = houseList;
            dialog.MaterialTypeComboBox.ItemsSource = typeNameList;

            ContentDialogResult dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Secondary) return;

            updatePage();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            flag = 0;

            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            pageNumText = currentPage.ToString() + "/" + totalPage.ToString();
            PageNumberTextBlock.Text = pageNumText;
            datagrid.ItemsSource = materialList;
        }

        private void HouseManagementButton_Click(object sender, RoutedEventArgs e)
        {
            HouseManagementWindow window = new HouseManagementWindow();
            window.Show();
        }

        private void TypeManagementButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintAccount_Click(object sender, RoutedEventArgs e)
        {
            PrintAccountDialog dialog = new PrintAccountDialog();
            dialog.ShowAsync();
        }
    }
}