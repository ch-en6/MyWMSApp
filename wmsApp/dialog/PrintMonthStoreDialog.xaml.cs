using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows;
using WindowsFormsApp1.dto;
using wms;
using wms.utils;
using wmsApp.param;

namespace wmsApp.dialog
{
    /// <summary>
    /// PrintMonthDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PrintMonthStoreDialog : ContentDialog
    {
        public PrintMonthStoreDialog()
        {
            InitializeComponent();
        }

        public bool IsValidDate(string year, string month)
        {
            int parsedYear;
            if (year.Length != 4 || !int.TryParse(year, out parsedYear))
            {
                return false;
            }
            int parsedMonth = int.Parse(month);
            if(parsedMonth < 1 || parsedMonth > 12)
            {
                return false;
            }
            return true;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string year = YearTextBox.Text;
            string month = MonthTextBox.Text;
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                args.Cancel = true;
                MessageBox.Show("请填写日期");
            }
            else if (!IsValidDate(year, month))
            {
                args.Cancel= true;
                MessageBox.Show("输入日期无效！");
            }
            else
            {               
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
                            storeTime = param.storeTime.ToString(),
                            userId = param.userId,
                            notes = param.notes
                        };
                        dataset.Add(data);
                    }

                    string ReportTitle = year + "年" + month + "月入库单";

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
    }
}
