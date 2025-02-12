﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (datagrid.SelectedCells.Count > 0)
            {
                // 获取当前选中的单元格的行和列索引
                int rowIndex = datagrid.Items.IndexOf(datagrid.CurrentItem);
                int columnIndex = datagrid.CurrentColumn.DisplayIndex;

                // 根据行和列索引获取单元格的值
                var cellInfo = datagrid.SelectedCells[0];
                var content = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
                if (content != null)
                {
                    string cellValue = content.Text;
                    // 在这里可以对获取到的单元格值进行处理
                    
                }
            }
        }
    }
}
