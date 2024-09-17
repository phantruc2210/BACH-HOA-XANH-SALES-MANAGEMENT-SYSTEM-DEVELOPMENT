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
    public partial class ThongKeHD : Form
    {
        KetNoi data = new KetNoi();
        public ThongKeHD()
        {
            InitializeComponent();
        }

        private void ThongKeHD_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLBH_BACHHOAXANH.BaoCao.ReportHD.rdlc";
            ReportDataSource rpd = new ReportDataSource();
            rpd.Name = "DataSetBHXView";
            rpd.Value = data.Table("select * from ViewHD");
            reportViewer1.LocalReport.DataSources.Add(rpd);
            this.reportViewer1.RefreshReport();
        }
    }
}
