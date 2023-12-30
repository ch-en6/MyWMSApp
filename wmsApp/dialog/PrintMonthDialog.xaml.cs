using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.param;
using Microsoft.Reporting.WinForms;
using System.IO;
using System;

namespace wmsApp.dialog
{
    /// <summary>
    /// PrintMonthDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PrintMonthDialog : ContentDialog
    {
        public PrintMonthDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string date = CalendarTextBox.Text;
            if (string.IsNullOrEmpty(date))
            {
                args.Cancel = true;
                MessageBox.Show("请选择日期");
            }
            else
            {
                string year = date.Substring(0, date.IndexOf("年")); // 获取年份
                string month = date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1);
               
                Result result = StoreApi.getStoreByDate(year, month);
                List<StoreDetailParam> list = JsonHelper.JsonToList<StoreDetailParam>(result.data.ToString());

                if (list.Count == 0)
                {
                    args.Cancel = true;
                    MessageBox.Show("该月没有入库信息");
                }
                else
                {
                    List<PrintStoreParam> dataset = new List<PrintStoreParam>();
                    foreach (var param in list)
                    {
                        PrintStoreParam data = new PrintStoreParam()
                        {
                            storeNo = param.storeNo,
                            materialName = param.material.name,
                            materialId = param.material.id,
                            materialType = param.material.type,
                            houseName = param.material.houseName,
                            storeCount = param.storeCount,
                            materialUnit = param.material.unit,
                            storeTime = param.storeTime.ToString("yyyy-MM-dd"),
                            userId = param.userId,
                            notes = param.notes
                        };
                        dataset.Add(data);
                    }

                    string ReportTitle = date + "入库单";

                    StoreWinForm form = new StoreWinForm(ReportTitle, dataset);
                    form.Show();
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

        private void Calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if(Calendar.DisplayMode == CalendarMode.Month || Calendar.DisplayMode == CalendarMode.Decade)
            {
                Calendar.DisplayMode = CalendarMode.Year;
                e.Handled = true;
            }
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            var year = Calendar.DisplayDate.Date.Year;
            var month = Calendar.DisplayDate.Date.Month;
            CalendarTextBox.Text = year + "年" + month + "月";
            CalendarPop.IsOpen = false;
        }

        private void CalendarImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CalendarPop.IsOpen)
            {
                CalendarPop.IsOpen = false;
            }
            else
            {
                CalendarPop.IsOpen = true;
            }
        }
    }
}
