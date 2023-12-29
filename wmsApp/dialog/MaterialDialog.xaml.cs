using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Forms;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.pojo;
//using static wmsApp.pages.StorePage;

namespace wmsApp.dialog
{
    /// <summary>
    /// DeliverDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialDialog : ContentDialog
    {
        public MaterialDialog()
        {
            // 其他初始化逻辑
            InitializeComponent();



        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }    
}
