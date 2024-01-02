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
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms.utils;
using wms;
using wms.pojo;

using ModernWpf.Controls;
using wmsApp.pages;
using System.Xml.Linq;
using wmsApp.pojo;

namespace wmsApp.dialog
{   
    public partial class MaterialWindow : Window
    {
        private AddDeliverPage AddDeliverPageInstance;
        private AddStorePage AddStorePageInstance;
        int currentPage = 1;
        long totalPage = 0;
        int flag = 0;
        int pageFlag = 0;
        public MaterialWindow(AddDeliverPage addDeliverPageInstance)
        {

            // 其他初始化逻辑
            InitializeComponent(); flag = 0;
            pageFlag = 0;
            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
            this.AddDeliverPageInstance = addDeliverPageInstance;
        }

        public MaterialWindow(AddStorePage addStorePageInstance)
        {
            // 其他初始化逻辑
            InitializeComponent(); flag = 0;
            pageFlag = 1;
            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            InitializeComponent();
            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
            this.AddStorePageInstance = addStorePageInstance;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // 获取MaterialWindow的DataGrid中选定的项
            Material selectedItem = datagrid.SelectedItem as Material;

            // 关闭MaterialWindow
            this.Close();

            long id = selectedItem.id;
            string type = selectedItem.type;
            string name = selectedItem.name;
            string houseName = selectedItem.houseName;
            int stock = selectedItem.stock;

            IOMaterial ioMaterial = new IOMaterial(id, type, name, houseName, stock,0,null);
            if(pageFlag == 0)
            {
                AddDeliverPageInstance.AddNewRow(ioMaterial);
            }
            else if(pageFlag == 1)
            {
                AddStorePageInstance.AddNewRow(ioMaterial);
            }
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

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            flag = 0;

            Result result = MaterialApi.search(currentPage);
            List<Material> materialList = JsonHelper.JsonToList<Material>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = materialList;
        }


    }
}

