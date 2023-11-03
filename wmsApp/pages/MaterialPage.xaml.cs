using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;

namespace wmsApp.pages
{
    public partial class MaterialPage : Page
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
        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            // 处理 CurrentCellChanged 事件的逻辑

            if (datagrid.CurrentCell != null)
            {
                int rowIndex = datagrid.SelectedIndex;
                int columnIndex = datagrid.CurrentCell.Column.DisplayIndex;

                if (rowIndex >= 0 && rowIndex < datagrid.Items.Count)
                {
                    object rowItem = datagrid.Items[rowIndex];
                    var cellContent = datagrid.Columns[columnIndex].GetCellContent(rowItem);

                    if (cellContent is TextBlock textBlock)
                    {
                        string cellValue = textBlock.Text;

                        // 在消息框中显示单元格的值
                        System.Windows.MessageBox.Show(cellValue);
                    }
                    else
                    {
                        // 单元格不包含 TextBlock，处理其他情况
                        // ...
                    }
                }
                else
                {
                    // 没有选中任何行
                    System.Windows.MessageBox.Show("没有选定单元格");
                }
            }
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
    }
}
