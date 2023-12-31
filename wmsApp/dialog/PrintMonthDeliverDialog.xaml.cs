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
using wmsApp.param;

namespace wmsApp.dialog
{
    /// <summary>
    /// PrintMonthDeliverDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PrintMonthDeliverDialog : ContentDialog
    {
        public PrintMonthDeliverDialog()
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
            if (parsedMonth < 1 || parsedMonth > 12)
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
                args.Cancel = true;
                MessageBox.Show("输入日期无效！");
            }
            else
            {
                Result result = DeliverApi.getDeliverByDate(year, month);
                List<DeliverDetailParam> list = JsonHelper.JsonToList<DeliverDetailParam>(result.data.ToString());

                if (list.Count == 0)
                {
                    args.Cancel = true;
                    MessageBox.Show("该月没有入库信息");
                }
                else
                {
                    List<PrintDeliverParam> dataset = new List<PrintDeliverParam>();
                    foreach (var param in list)
                    {
                        PrintDeliverParam data = new PrintDeliverParam()
                        {
                            deliverNo = param.deliverNo,
                            materialName = param.material.name,
                            materialId = param.material.id,
                            materialType = param.material.type,
                            houseName = param.material.houseName,
                            deliverCount = param.deliverCount,
                            materialUnit = param.material.unit,
                            deliverTime = param.deliverTime.ToString(),
                            userId = param.userId,
                            notes = param.notes
                        };
                        dataset.Add(data);
                    }

                    string ReportTitle = year + "年" + month + "月出库单";

                    DeliverWinForm form = new DeliverWinForm(ReportTitle, dataset);
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
