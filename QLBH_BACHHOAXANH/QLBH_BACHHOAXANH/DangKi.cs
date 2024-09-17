using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BACHHOAXANH
{
    public partial class DangKi : Form
    {
        KetNoi data = new KetNoi();
        public DangKi()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            DangNhap dn = new DangNhap();
            dn.ShowDialog();
        }

        private void btnDangKi_Click(object sender, EventArgs e)
        {
            try
            {
                string dangNhap = txtTenDN.Text;
                string matKhau = txtMatKhau.Text;
                string nhapmatKhau = txtNhapMK.Text;
                string chucVu = cboxChucVu.Text;
                string manv = txtMaNV.Text;

                string slq = "select * from TaiKhoan where TenDN = '" + dangNhap + "'";
                SqlCommand cmd = new SqlCommand(slq, data.GetConnect());
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == false)
                {
                    if (matKhau == nhapmatKhau)
                    {
                        data.ExecuteNonQuery("insert into TaiKhoan(TenDN, MatKhau, ChucVu, MaNV) values ( N'"+ dangNhap + "', N'"
                            + matKhau + "', N'" + chucVu + "', '" + manv + "')");
                        MessageBox.Show("Đăng kí thành công", "Đăng kí", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        DangNhap dn = new DangNhap();
                        dn.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Nhập lại mật khẩu. Mật khẩu không trùng nhau !", "Đăng kí", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập bị trùng !", "Đăng kí", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng kí không thành công ! Lỗi " + ex.Message, "Đăng kí", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
