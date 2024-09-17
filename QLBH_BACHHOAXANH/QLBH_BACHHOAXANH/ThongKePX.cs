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

namespace QLBH_BACHHOAXANH
{
    public partial class ThongKePX : Form
    {
        KetNoi data = new KetNoi();
        public ThongKePX()
        {
            InitializeComponent();
        }

        private void ThongKePX_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLBH_BACHHOAXANH.BaoCao.ReportPX.rdlc";
            ReportDataSource rpd = new ReportDataSource();
            rpd.Name = "DataSetBHXView";
            rpd.Value = data.Table("select * from ViewPX");
            reportViewer1.LocalReport.DataSources.Add(rpd);
            this.reportViewer1.RefreshReport();
        }
    }
}
