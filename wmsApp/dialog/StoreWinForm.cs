using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wmsApp.param;

namespace wmsApp.dialog
{
    public partial class StoreWinForm : Form
    {
        private string ReportTitle;
        private string User;
        private List<PrintStoreParam> dataset = new List<PrintStoreParam>();
        public StoreWinForm()
        {
            InitializeComponent();
            this.TopMost = true;

        }

        public StoreWinForm(string title, List<PrintStoreParam> list)
        {
            InitializeComponent();
            this.ReportTitle = title;
            //this.User = user;
            this.dataset = list;

            this.TopMost = true;
            this.Width = 1600;
            this.Height = 860;
            this.StartPosition = FormStartPosition.CenterScreen;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            int totalStoreNo = dataset.Count;
            int totalStoreCount = dataset.Sum(s => s.storeCount);
            
            //ReportParameter parameter2 = new ReportParameter("user", User);
            List<ReportParameter> parameters = new List<ReportParameter>
            {
                new ReportParameter("Title", ReportTitle),
                new ReportParameter("TotalStoreNo", totalStoreNo.ToString()),
                new ReportParameter("TotalStoreCount", totalStoreCount.ToString())
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
