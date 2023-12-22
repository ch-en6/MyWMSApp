using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Windows;

using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wmsApp.controls;
using wmsApp.pojo;

namespace wmsApp.dialog
{
    /// <summary>
    /// DeliverDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DeliverDialog : ContentDialog
    {
        private List<Deliver> dataList;
        public DeliverDialog()
        {
            //InitializeComponent();   
            //List<Deliver> list = new List<Deliver>();
            //datagrid.ItemsSource = list;
            InitializeComponent();
            dataList = new List<Deliver>();
            datagrid.ItemsSource = dataList;
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void datagrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            dataList.Add(new Deliver());
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = dataList;
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem is Deliver selectedDeliver)
            {
                dataList.Remove(selectedDeliver);
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = dataList;
            }
        }

    }
}
