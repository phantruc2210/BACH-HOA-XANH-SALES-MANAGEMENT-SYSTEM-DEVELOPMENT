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
    public partial class DangNhap : Form
    {
        KetNoi data = new KetNoi();
        public DangNhap()
        {
            InitializeComponent();
        }
        static void Main()
        {
            DangNhap dn = new DangNhap();
            Application.Run(dn);
        }

        private void btnDangKi_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangKi dk = new DangKi();
            dk.Show();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {

                if(txtTenDN.Text == "quanly" && txtMatKhau.Text == "123456789")
                {
                    MessageBox.Show("Đăng nhập thành công !", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    TrangChu main = new TrangChu();
                    main.Show();
                }
                else
                {
                    string dn = txtTenDN.Text;
                    string mk = txtMatKhau.Text;

                    string sql = "select * from TaiKhoan where TenDN = N'" + dn + "' and MatKhau = '" + mk + "'";
                    SqlCommand cmd = new SqlCommand(sql, data.GetConnect());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        MessageBox.Show("Đăng nhập thành công !", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        TrangChuNV main = new TrangChuNV();
                        main.Show();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập/ mật khẩu sai. Đăng nhập không thành công !", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập không thành công ! Lỗi " + ex.Message, "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
