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
    public partial class DoiMatKhau : Form
    {
        KetNoi data = new KetNoi();
        public DoiMatKhau()
        {
            InitializeComponent();
        }
        static void Main()
        {
            DoiMatKhau dmk = new DoiMatKhau();
            Application.Run(dmk);
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            try
            {
                string dn = txtTenDN.Text;
                string mk = txtMKCu.Text;

                string sql = "select * from TaiKhoan where TenDN = N'" + dn + "' and MatKhau ='" + mk + "'";
                SqlCommand cmd = new SqlCommand(sql, data.GetConnect());
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    if (txtMKMoi.Text == txtNhapMKMoi.Text)
                    {
                        data.ExecuteNonQuery(@"update TaiKhoan set MatKhau ='" + txtMKMoi.Text + "'where TenDN = N'" + dn + "' and MatKhau = '" + txtMKCu.Text + "'");
                        MessageBox.Show(" Đổi mật khẩu thành công !", "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nhập lại mật khẩu mới sai !", "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
                }
                else
                {
                    MessageBox.Show("Mật khẩu hoặc tên đăng nhập sai !", "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đổi mật khẩu không thành công! Lỗi " + ex.Message, "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
