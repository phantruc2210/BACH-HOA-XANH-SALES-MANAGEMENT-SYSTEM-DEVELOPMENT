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
    public partial class BaoCao : Form
    {
        public BaoCao()
        {
            InitializeComponent();
        }
        static void Main()
        {
            BaoCao baoCao = new BaoCao();
            Application.Run(baoCao );
        }    

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radHD.Checked && !radPN.Checked && !radPX.Checked
            && !radNCC.Checked && !radSP.Checked && !radHDKD.Checked)
            {
                ThongKeHD thongKe_HD = new ThongKeHD();
                thongKe_HD.ShowDialog();
            }
            else if (!radHD.Checked && radPN.Checked && !radPX.Checked
            && !radNCC.Checked && !radSP.Checked && !radHDKD.Checked)
            {
                ThongKePN thongKe_PN = new ThongKePN();
                thongKe_PN.ShowDialog();
            }
            else if (!radHD.Checked && !radPN.Checked && radPX.Checked
            && !radNCC.Checked && !radSP.Checked && !radHDKD.Checked)
            {
                ThongKePX thongKe_PX = new ThongKePX();
                thongKe_PX.ShowDialog();
            }
            else if (!radHD.Checked && !radPN.Checked && !radPX.Checked
            && !radNCC.Checked && radSP.Checked && !radHDKD.Checked)
            {
                ThongKeSP thongKe_SP = new ThongKeSP();
                thongKe_SP.ShowDialog();
            }
            else if (!radHD.Checked && !radPN.Checked && !radPX.Checked
            && radNCC.Checked && !radSP.Checked && !radHDKD.Checked)
            {
                ThongKeNCC thongKe_NCC = new ThongKeNCC();
                thongKe_NCC.ShowDialog();
            }
            else if (!radHD.Checked && !radPN.Checked && !radPX.Checked
            && !radNCC.Checked && !radSP.Checked && radHDKD.Checked)
            {
                ThongKeHDKD thongKe_HDKD = new ThongKeHDKD();
                thongKe_HDKD.ShowDialog();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
