using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.dialog;

namespace wmsApp.pages
{
    public partial class MaterialPage : System.Windows.Controls.Page
    {
        int currentPage = 1;
        long totalPage = 0;
        int flag = 0;

        public MaterialPage()
        {
            flag = 0;
            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
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
                    result = searchHouseId(textBox.Text);
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

            PageNumberTextBlock.Text = currentPage.ToString();
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
            long.TryParse(userInput, out id);
            Result result = MaterialApi.searchById(currentPage, id);
            return result;
        }

        private Result searchName(string userInput)
        {
            if(userInput == "")
            {
                userInput = "...";
            }
            Result result = MaterialApi.searchByName(currentPage, userInput);
            return result;
        }

        private Result searchHouseId(string userInput)
        {
            long id;
            long.TryParse(userInput, out id);
            Result result = MaterialApi.searchByHouseId(currentPage, id);
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
                case "meterialHouseID":
                    flag = 3;
                    result = searchHouseId(userInput);
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

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
        }

        private async void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = datagrid.SelectedItem as Material;

            Result houseIdResult = MaterialApi.searchHouseId();
            List<string> houseList = JsonHelper.JsonToList<string>(houseIdResult.data.ToString());
            Result typeNameResult = MaterialApi.searchTypeName();
            List<string> typeName = JsonHelper.JSONToList<string>(typeNameResult.data.ToString());

            // 将数据分配给对应的TextBox或ComboBox
            UpdateMaterialDialog dialog = new UpdateMaterialDialog();
            dialog.MaterialIdTextBox.Text = selectedItem.id.ToString();
            dialog.MaterialNameTextBox.Text = selectedItem.name;
            dialog.MaterialStockTextBox.Text = selectedItem.stock.ToString();
            dialog.MaterialCommentsTextBox.Text = selectedItem.comments;
            dialog.MaterialCreTimeTextBox.Text = selectedItem.createTime.ToString("yyyy-MM-dd HH:mm:ss");

            dialog.MaterialHouseIdComboBox.ItemsSource = houseList;
            dialog.MaterialHouseIdComboBox.SelectedValue = selectedItem.houseId.ToString();

            dialog.MaterialTypeComboBox.ItemsSource = typeName;
            dialog.MaterialTypeComboBox.SelectedValue = selectedItem.type;

            dialog.MAterialUnitComboBox.Text = selectedItem.unit;

            ContentDialogResult dialogResult = await dialog.ShowAsync();
            if (dialogResult == ContentDialogResult.Secondary) return;

            updatePage();

        }

        private void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddMaterialButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
