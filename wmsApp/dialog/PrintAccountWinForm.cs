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
using wms.pojo;
using wmsApp.param;

namespace wmsApp.dialog
{
    public partial class PrintAccountWinForm : Form
    {
        private string Year;
        private Material material;
        private List<PrintAccountParam> printAcounts = new List<PrintAccountParam>();

        public PrintAccountWinForm()
        {
            InitializeComponent();
        }

        public PrintAccountWinForm(string Year, Material material, List<PrintAccountParam> accountDetials)
        {
            InitializeComponent();
            this.Year = Year;
            this.material = material;
            printAcounts = accountDetials;

            this.TopMost = true;
            this.Width = 1600;
            this.Height = 860;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void PrintAccountWinForm_Load(object sender, EventArgs e)
        {
            string title = Year + "年" + material.name + "出入库报表";
            List<ReportParameter> parameters = new List<ReportParameter>()
            {
                new ReportParameter("Title", title),
                new ReportParameter("materialName", material.name),
                new ReportParameter("materialID", material.id.ToString()),
                new ReportParameter("materialType", material.type),
                new ReportParameter("materialUnit", material.unit),
                new ReportParameter("totalId", printAcounts.Count.ToString())
            };
            reportViewer1.LocalReport.SetParameters(parameters);
            ReportDataSource reportDataSource = new ReportDataSource("account", printAcounts);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
