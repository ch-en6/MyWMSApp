using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.dto;
using wms.utils;
using wms;
using wmsApp.param;
using wms.pojo;

namespace wmsApp.dialog
{
    public partial class DeliverWinForm : Form
    {
        private string ReportTitle;
        private List<PrintDeliverParam> dataset = new List<PrintDeliverParam>();

        public DeliverWinForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public DeliverWinForm(string title, List<PrintDeliverParam> list)
        {
            InitializeComponent();
            this.ReportTitle = title;
            this.dataset = list;

            this.TopMost = true;
            this.Width = 1600;
            this.Height = 860;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void DeliverWinForm_Load(object sender, EventArgs e)
        {
            int totalDeliverNo = dataset.Select(d => d.deliverNo).Distinct().Count(); ;
            int totalDeliverCount = dataset.Sum(d => d.deliverCount);
            string userName = "无";
            Result userResult = UserApi.getNowUser();
            if (userResult.success)
            {
                User user = JsonHelper.JSONToObject<User>(userResult.data.ToString());
                userName = user.name;
            }


            List<ReportParameter> parameters = new List<ReportParameter>
            {
                new ReportParameter("Title", ReportTitle),
                new ReportParameter("UserName", userName),
                new ReportParameter("TotalDeliverNo", totalDeliverNo.ToString()),
                new ReportParameter("TotalDeliverCount", totalDeliverCount.ToString())
            };
            reportViewer1.LocalReport.SetParameters(parameters);

            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dataset);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            this.reportViewer1.RefreshReport();

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
