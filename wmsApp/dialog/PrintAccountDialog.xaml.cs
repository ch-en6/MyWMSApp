using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.param;
using wmsApp.pojo;

namespace wmsApp.dialog
{
    /// <summary>
    /// RrintAccountDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PrintAccountDialog : ContentDialog
    {
        public PrintAccountDialog()
        {
            InitializeComponent();
        }

        public bool IsValidYear(string year)
        {
            int parsedYear;
            if (year.Length != 4 || !int.TryParse(year, out parsedYear))
            {
                return false;
            }
            return true;
        }

        public void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string year = YearTextBox.Text;
            string materialName = MaterialNameTextBox.Text;
            string houseName = houseNameComboBox.Text;

            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(houseName))
            {
                args.Cancel = true;
                MessageBox.Show("请输入完整信息！");
            }
            else if (!IsValidYear(year))
            {
                args.Cancel = true;
                MessageBox.Show("无效的年份，请重新输入！");
            }
            else
            {
                Result materialResult = MaterialApi.getMaterialByNameAndHouse(materialName, houseName);
                if (materialResult.success)
                {
                    Material material = JsonHelper.JSONToObject<Material>(materialResult.data.ToString());

                    Result storeResult = StoreApi.getStoreByYear(year, material);
                    List<Store> stores = new List<Store>();
                    if (storeResult.success)
                    {
                        stores = JsonHelper.JsonToList<Store>(storeResult.data.ToString());
                    }

                    Result deliverResult = DeliverApi.getDeliverByYear(year, material);
                    List<Deliver> delivers = new List<Deliver>();
                    if (deliverResult.success)
                    {
                        delivers = JsonHelper.JsonToList<Deliver>(deliverResult.data.ToString());
                    }

                    if (stores.Count < 1 && delivers.Count < 1)
                    {
                        args.Cancel = true;
                        MessageBox.Show("没有信息！");
                    }
                    else
                    {
                        List<PrintAccountParam> accountDetials = ConvertToAccountDetailFormParam(stores, delivers);
                        PrintAccountWinForm form = new PrintAccountWinForm(year, material, accountDetials);
                        form.ShowDialog();
                    }
                }
                else
                {
                    args.Cancel = true;
                    MessageBox.Show("没有信息！");
                }
            }
        }

        public List<PrintAccountParam> ConvertToAccountDetailFormParam(List<Store> stores, List<Deliver> delivers)
        {
            List<PrintAccountParam> accountDetials = new List<PrintAccountParam>();
            if (stores.Count > 0)
            {
                foreach (Store store in stores)
                {
                    PrintAccountParam account = new PrintAccountParam
                    {
                        Id = store.storeId,
                        Type = "入库",
                        No = store.storeNo,
                        count = store.storeCount,
                        remainCount = store.remainCount,
                        dateTime = store.storeTime,
                        uerId = store.userId,
                        notes = store.notes
                    };
                    accountDetials.Add(account);
                }
            }
            if(delivers.Count > 0 )
            {
                foreach (Deliver deliver in delivers)
                {
                    PrintAccountParam account = new PrintAccountParam
                    {
                        Id = deliver.DeliverId,
                        Type = "出库",
                        No = deliver.DeliverNo,
                        count = deliver.DeliverCount,
                        remainCount = deliver.DeliverCount,
                        dateTime = deliver.DeliverTime,
                        uerId = deliver.UserId,
                        notes = deliver.Notes
                    };
                    accountDetials.Add(account);
                }
            }
            accountDetials = accountDetials.OrderBy(a => a.dateTime).ToList();
            for(int i = 0; i < accountDetials.Count; i++)
            {
                accountDetials[i].Id = i + 1;
            }
            return accountDetials;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 关闭对话框
            sender.Hide();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string year = YearTextBox.Text;
            string materialName = MaterialNameTextBox.Text;

            if (string.IsNullOrEmpty(materialName))
            {
                MessageBox.Show("请输入物料名！");
                return;
            }
            else
            {
                if(!IsValidYear(year))
                {
                    MessageBox.Show("无效的年份，请重新输入！");
                    return;
                }
                Result houseResult = MaterialApi.houseByYearAndMaterialName(year, materialName);
                if (houseResult.success)
                {
                    List<string> houseName = JsonHelper.JsonToList<string>(houseResult.data.ToString());
                    houseNameComboBox.ItemsSource = houseName;
                    MessageBox.Show("查询结果请点击 仓库下拉框 ！");
                }
                else
                {
                    MessageBox.Show("无信息！");
                }
            }
        }
    }
}
