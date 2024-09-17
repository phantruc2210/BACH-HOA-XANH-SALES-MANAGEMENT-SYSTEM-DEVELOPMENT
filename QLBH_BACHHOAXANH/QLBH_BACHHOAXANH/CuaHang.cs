using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BACHHOAXANH
{
    public partial class CuaHang : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        public CuaHang()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinCH();
            dgvCuaHang.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // thay đổi độ rộng ô
            dgvCuaHang.Columns[1].Width = 220;
            dgvCuaHang.Columns[2].Width = 150;
            // màu dòng
            dgvCuaHang.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvCuaHang.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvCuaHang.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCuaHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CuaHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvCuaHang_SelectionChanged(object sender, EventArgs e)
        {
            txtMaCH.Text = dgvCuaHang.CurrentRow.Cells[0].Value.ToString();
            txtTenCH.Text = dgvCuaHang.CurrentRow.Cells[1].Value.ToString();
            txtCHTruong.Text = dgvCuaHang.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            bdsource.Position = 0;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();

            btnTruoc.Enabled = false;
            btnDau.Enabled = false;
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            bdsource.Position -= 1;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();

            if(bdsource.Position == 0)
            {
                btnTruoc.Enabled = false;
                btnDau.Enabled = false;
            }
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;
        }

        private void btnKe_Click(object sender, EventArgs e)
        {
            bdsource.Position += 1;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();

            if (bdsource.Position == bdsource.Count - 1)
            {
                btnKe.Enabled = false;
                btnCuoi.Enabled = false;
            }
            btnTruoc.Enabled = true;
            btnDau.Enabled = true;
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            bdsource.Position = bdsource.Count - 1;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();

            btnTruoc.Enabled = true;
            btnDau.Enabled = true;
            btnKe.Enabled = false;
            btnCuoi.Enabled = false;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaCH.Text = "";
            txtTenCH.Text = "";
            txtCHTruong.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string mach = txtMaCH.Text;
                string tench = txtTenCH.Text;
                string chtruong = txtCHTruong.Text;  

                DialogResult result = MessageBox.Show("Bạn muốn thêm cửa hàng ?", "Thêm cửa hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into CuaHang(MaCH, TenCH, CHTruong) values ('" + mach + "', N'"+ tench + "', N'"+ chtruong + "')");
                    MessageBox.Show("Thêm cửa hàng thành công!", "Thêm cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm cửa hàng không thành công ! Lỗi " + ex.Message, "Thêm cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {

                string mach = txtMaCH.Text;
                string tench = txtTenCH.Text;
                string chtruong = txtCHTruong.Text;


                DialogResult result = MessageBox.Show("Bạn muốn sửa cửa hàng ?", "Sửa cửa hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update CuaHang set TenCH = N'" + tench + "', CHTruong = N'" + chtruong + "' where MaCH = '"+ mach + "'");
                    MessageBox.Show("Sửa cửa hàng thành công!", "Sửa cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa cửa hàng không thành công ! Lỗi " + ex.Message, "Sửa cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {

                string mach = txtMaCH.Text;
                string tench = txtTenCH.Text;
                string chtruong = txtCHTruong.Text;


                DialogResult result = MessageBox.Show("Bạn muốn xóa cửa hàng ?", "Xóa cửa hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from CuaHang where MaCH = '" + mach + "'");
                    MessageBox.Show("Xóa cửa hàng thành công!", "Xóa cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa cửa hàng không thành công ! Lỗi " + ex.Message, "Xóa cửa hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string mach = txt_timMaCH.Text;
            string tench = txt_timTenCH.Text;
            if(radMaCH.Checked && !radTenCH.Checked)
            {
                if(mach != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from CuaHang where MaCH = '" + mach + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvCuaHang.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy cửa hàng ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
           else if (!radMaCH.Checked &&  radTenCH.Checked)
           {
                if(tench != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from CuaHang where TenCH like N'%" + tench + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvCuaHang.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy cửa hàng ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }    
        }

        private void btnKoLoc_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
