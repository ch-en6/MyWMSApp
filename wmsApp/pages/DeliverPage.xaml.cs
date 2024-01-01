﻿using ModernWpf.Controls;
using NuGet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.dialog;
using wmsApp.param;
using wmsApp.pojo;

namespace wmsApp.pages
{
    /// <summary>
    /// deliverPage.xaml 的交互逻辑
    /// </summary>
    public partial class DeliverPage : System.Windows.Controls.Page
    {
        int currentPage = 1;
        long totalPage = 0;
        int flag = 0;

        public DeliverPage()
        {
            InitializeComponent();

            flag = 0;

            SearchAll(currentPage);

            Result houseName = MaterialApi.searchHouseName();
            List<string> houseList = JsonHelper.JsonToList<string>(houseName.data.ToString());
            warehouseNameComboBox.ItemsSource = houseList;
        }

        public void SearchAll(int page)
        {
            Result result = DeliverApi.searchAll(page);
            List<DeliverDetailParam> deliverList = JsonHelper.JsonToList<DeliverDetailParam>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = deliverList;
        }

        public void ConditionSearch(int page)
        {
            long deliverNo = string.IsNullOrEmpty(deliverNoTextBox.Text) ? 0 : long.Parse(deliverNoTextBox.Text);
            long materialId = string.IsNullOrEmpty(materialIdTextBox.Text) ? 0 : long.Parse(materialIdTextBox.Text);
            string warehouseName = warehouseNameComboBox.Text;
            DateTime? startTime = startTimeTextBox.SelectedDate;
            DateTime? endTime = endTimeTextBox.SelectedDate;
            long operatorId = string.IsNullOrEmpty(operatorIdTextBox.Text) ? 0 : long.Parse(operatorIdTextBox.Text);
            string notes = deliverNotes.Text;

            if (startTime.HasValue && endTime.HasValue && endTime < startTime)
            {
                MessageBox.Show("结束时间不能早于开始时间，请重新选择");
                return;
            }

            Result result = DeliverApi.searchCondition(deliverNo, warehouseName, startTime, endTime, materialId, operatorId, notes, page);
            List<DeliverDetailParam> deliverList = JsonHelper.JsonToList<DeliverDetailParam>(result.data.ToString());
            totalPage = result.total;

            PageNumberTextBlock.Text = currentPage.ToString();
            datagrid.ItemsSource = deliverList;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;

            currentPage = 1;
            ConditionSearch(currentPage);
        }

        public void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            SearchAll(currentPage);
        }

        public void btnClear_Click(object sender, RoutedEventArgs e)
        {
            deliverNoTextBox.Text = "";
            materialIdTextBox.Text = "";
            warehouseNameComboBox.SelectedIndex = -1;
            startTimeTextBox.SelectedDate = null;
            endTimeTextBox.SelectedDate = null;
            operatorIdTextBox.Text = "";
            deliverNotes.Text = "";
        }

        public async void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/pages/AddDeliverPage.xaml", UriKind.Relative));
        }

        public void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintMonthDeliverDialog dialog = new PrintMonthDeliverDialog();
            dialog.ShowAsync();
        }


        public void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                if (flag == 0)
                {
                    SearchAll(currentPage);
                }
                if (flag == 1)
                {
                    ConditionSearch(currentPage);
                }
            }

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                if (flag == 0)
                {
                    SearchAll(currentPage);
                }
                if (flag == 1)
                {
                    ConditionSearch(currentPage);
                }
            }
        }

    }
}