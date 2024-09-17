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
    public partial class KhachHang : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        public KhachHang()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinKH();
            dgvKhachHang.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // thay đổi độ rộng ô
            dgvKhachHang.Columns[0].Width = 50;
            dgvKhachHang.Columns[1].Width = 70;
            dgvKhachHang.Columns[5].Width = 70;
            // màu dòng
            dgvKhachHang.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvKhachHang.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvKhachHang.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // nạp dữ liệu combo box thành viên
            cboxThanhVien.Items.Clear();
            foreach (DataRowView row in bdsource)
            {
                string tvien = row["ThanhVien"].ToString();
                if (!cboxThanhVien.Items.Contains(tvien))
                {
                    cboxThanhVien.Items.Add(tvien);
                }

            }

        }
        private void KhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiaChi.Text = "";
            dtpNgaySinh.Value = DateTime.Today;
            txtDienThoai.Text = "";
            cboxThanhVien.Text = string.Empty;

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

            if (bdsource.Position == 0)
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

        private void dgvKhachHang_SelectionChanged(object sender, EventArgs e)
        {
            txtMaKH.Text = dgvKhachHang.CurrentRow.Cells[0].Value.ToString();
            txtTenKH.Text = dgvKhachHang.CurrentRow.Cells[1].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.CurrentRow.Cells[2].Value.ToString();
            dtpNgaySinh.Value = (DateTime)dgvKhachHang.CurrentRow.Cells[3].Value;
            txtDienThoai.Text = dgvKhachHang.CurrentRow.Cells[4].Value.ToString();
            cboxThanhVien.SelectedItem = dgvKhachHang.CurrentRow.Cells["ThanhVien"].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string makh = txtMaKH.Text;
                string tenkh = txtTenKH.Text;
                string diachi = txtDiaChi.Text;
                string nsinh = dtpNgaySinh.Value.ToString("MM/dd/yyyy");
                string dthoai = txtDienThoai.Text;
                string tvien = cboxThanhVien.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm khách hàng ?", "Thêm khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into KhachHang(MaKH, TenKH, DiaChi, NgaySinh, DienThoai, ThanhVien)" + 
                        " values ('"+ makh + "', N'" + tenkh + "', N'" + diachi + "' , '"+ nsinh + "' , '"+ dthoai + "', '" + tvien + "')");
                    MessageBox.Show("Thêm khách hàng thành công!", "Thêm khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm khách hàng không thành công ! Lỗi " + ex.Message, "Thêm khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string makh = txtMaKH.Text;
                string tenkh = txtTenKH.Text;
                string diachi = txtDiaChi.Text;
                string nsinh = dtpNgaySinh.Value.ToString("MM/dd/yyyy");
                string dthoai = txtDienThoai.Text;
                string tvien = cboxThanhVien.Text;


                DialogResult result = MessageBox.Show("Bạn muốn sửa khách hàng ?", "Sửa khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update KhachHang set TenKH = N'" + tenkh + "', DiaChi = N'" + diachi + "' ," +
                        "NgaySinh = '" + nsinh + "' , DienThoai = '" + dthoai + "', ThanhVien = '" + tvien + "' where MaKH = '"+ makh + "'");
                    MessageBox.Show("Sửa khách hàng thành công!", "Sửa khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa khách hàng không thành công ! Lỗi " + ex.Message, "Sửa khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string makh = txtMaKH.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa khách hàng ?", "Xóa khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from KhachHang where MaKH = '" + makh + "'");
                    MessageBox.Show("Xóa khách hàng thành công!", "Xóa khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa khách hàng không thành công ! Lỗi " + ex.Message, "Xóa khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string makh = txt_timMaKH.Text;
            string tenkh = txt_timTenKH.Text;
            if (radMaKH.Checked && !radTenKH.Checked)
            {
                if(makh != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from KhachHang where MaKH = '" + makh + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvKhachHang.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvKhachHang.Columns[5].Width = 100;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }   
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
                

            }
            else if (!radMaKH.Checked && radTenKH.Checked)
            {
                if(tenkh != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from KhachHang where TenKH like N'%" + tenkh + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvKhachHang.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvKhachHang.Columns[5].Width = 100;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
