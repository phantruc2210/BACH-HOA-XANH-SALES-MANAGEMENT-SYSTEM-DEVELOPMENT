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
    public partial class TrangChuNV : Form
    {
        //khái báo biến về form hiện tại là form trang chủ
        private Form currentFrch;
        public TrangChuNV()
        {
            InitializeComponent();
        }
        public void OpenChildForm(Form currentFrch, Panel Panel_Body, Form frch)
        {
            if (currentFrch != null)
            {
                currentFrch.Close();
            }
            currentFrch = frch;
            frch.TopLevel = false;
            frch.FormBorderStyle = FormBorderStyle.None;
            frch.Dock = DockStyle.Fill;
            Panel_Body.Controls.Add(frch);
            Panel_Body.Tag = frch;
            frch.BringToFront();
            frch.Show();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn đăng xuất ?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                DangNhap dn = new DangNhap();
                dn.ShowDialog();

            }
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            DoiMatKhau doiMatKhau = new DoiMatKhau();
            OpenChildForm(currentFrch, Panel_Body, doiMatKhau);
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            KhachHang khachHang = new KhachHang();
            OpenChildForm(currentFrch, Panel_Body, khachHang);
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            NhaCungCap nhaCungCap = new NhaCungCap();
            OpenChildForm(currentFrch, Panel_Body, nhaCungCap);
        }

        private void btnLoaiSanPham_Click(object sender, EventArgs e)
        {
            LoaiSanPham loaiSanPham = new LoaiSanPham();
            OpenChildForm(currentFrch, Panel_Body, loaiSanPham);
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            SanPham sanPham = new SanPham();
            OpenChildForm(currentFrch, Panel_Body, sanPham);
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            PhieuNhap phieuNhap = new PhieuNhap();
            OpenChildForm(currentFrch, Panel_Body, phieuNhap);
        }

        private void btnPhieuXuat_Click(object sender, EventArgs e)
        {
            PhieuXuat phieuXuat = new PhieuXuat();
            OpenChildForm(currentFrch, Panel_Body, phieuXuat);
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            HoaDon hoaDon = new HoaDon();
            OpenChildForm(currentFrch, Panel_Body, hoaDon);
        }

        private void Pnel_Pic_MouseClick(object sender, MouseEventArgs e)
        {
            FormMain formMain = new FormMain();
            OpenChildForm(currentFrch, Panel_Body, formMain);
        }

        private void btnTroGiup_Click(object sender, EventArgs e)
        {
            TroGiup troGiup = new TroGiup();
            OpenChildForm(currentFrch, Panel_Body, troGiup);
        }
    }
}
