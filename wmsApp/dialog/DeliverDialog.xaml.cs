using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
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
        public DeliverDialog()
        {
            InitializeComponent();   
            List<Deliver> list = new List<Deliver>();
            datagrid.ItemsSource = list;
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
    }
}
