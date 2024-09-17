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
    public partial class HoaDon : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        private BindingSource bdsourceCTHD = new BindingSource();
        private BindingSource bdsourceNV = new BindingSource();
        private BindingSource bdsourceKH = new BindingSource();
        public HoaDon()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinHD();
            dgvHD.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // màu dòng
            dgvHD.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvHD.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvHD.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvHD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // thay đổi độ rộng ô
            dgvHD.Columns[0].Width = 100;
            dgvHD.Columns[1].Width = 100;
            dgvHD.Columns[2].Width = 100;
            dgvHD.Columns[3].Width = 100;
            dgvHD.Columns[4].Width = 100;

            // nạp dữ liệu combo box mã nhân viên
            bdsourceNV.DataSource = data.ThongTinNV();
            cboxMaNV.Items.Clear();
            foreach (DataRowView row in bdsourceNV)
            {
                string idnv = row["MaNV"].ToString();
                if (!cboxMaNV.Items.Contains(idnv))
                {
                    cboxMaNV.Items.Add(idnv);
                }

            }
            // nạp dữ liệu combo box mã khách hàng
            bdsourceKH.DataSource = data.ThongTinKH();
            cboxMaKH.Items.Clear();
            foreach (DataRowView row in bdsourceKH)
            {
                string idkh = row["MaKH"].ToString();
                if (!cboxMaKH.Items.Contains(idkh))
                {
                    cboxMaKH.Items.Add(idkh);
                }

            }

        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvHD_SelectionChanged(object sender, EventArgs e)
        {
            txtMaHD.Text = dgvHD.CurrentRow.Cells[0].Value.ToString();
            dtpNgayDH.Value = (DateTime)dgvHD.CurrentRow.Cells[1].Value;
            txtPTTT.Text = dgvHD.CurrentRow.Cells[2].Value.ToString();
            cboxMaNV.SelectedItem = dgvHD.CurrentRow.Cells["MaNV"].Value.ToString();
            cboxMaKH.SelectedItem = dgvHD.CurrentRow.Cells["MaKH"].Value.ToString();

            // HIỂN THỊ CHI TIẾT HÓA ĐƠN
            bdsourceCTHD.DataSource = data.ThongTinCTHD(txtMaHD.Text);
            dgvCTHD.DataSource = bdsourceCTHD;
            dgvCTHD.Columns[0].Width = 130;
            dgvCTHD.Columns[1].Width = 130;
            dgvCTHD.Columns[2].Width = 130;
            dgvCTHD.Columns[3].Width = 130;

            // tính tổng tiền trên mỗi hóa đơn
            double tong = data.TienTrenHD(txtMaHD.Text);
            txtTongTien.Text = String.Format("{0:0,000 VND}", tong);

            
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

        private void btnThemCTHD_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTHD.CurrentCell.RowIndex;
                string mahd = dgvCTHD.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTHD.Rows[vt].Cells[1].Value.ToString();
                string sld = dgvCTHD.Rows[vt].Cells[2].Value.ToString();
                string dongiaban = dgvCTHD.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn thêm chi tiết hóa đơn ?", "Thêm chi tiết hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into CTHoaDon(MaHD, MaSP, SoLuongDat, DGBan)" +
                        " values ('" + mahd + "', '" + masp + "', '" + sld + "' , '" + dongiaban + "')");
                    MessageBox.Show("Thêm chi tiết hóa đơn thành công!", "Thêm chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm chi tiết hóa đơn không thành công ! Lỗi " + ex.Message, "Thêm chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaCTHD_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTHD.CurrentCell.RowIndex;
                string mahd = dgvCTHD.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTHD.Rows[vt].Cells[1].Value.ToString();
                string sld = dgvCTHD.Rows[vt].Cells[2].Value.ToString();
                string dongiaban = dgvCTHD.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn sửa chi tiết hóa đơn ?", "Sửa chi tiết hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update CTHoaDon set SoLuongDat = '" + sld + "' , DGBan = '" + dongiaban + "' where MaHD = '" + mahd + "' and MaSP = '" + masp + "'");
                    MessageBox.Show("Sửa chi tiết hóa đơn thành công!", "Sửa chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa chi tiết hóa đơn không thành công ! Lỗi " + ex.Message, "Sửa chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaCTHD_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTHD.CurrentCell.RowIndex;
                string mahd = dgvCTHD.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTHD.Rows[vt].Cells[1].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn muốn xóa chi tiết hóa đơn ?", "Xóa chi tiết hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from CTHoaDon where MaHD = '" + mahd + "' and MaSP = '" + masp + "'");
                    MessageBox.Show("Xóa chi tiết hóa đơn thành công!", "Xóa chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa chi tiết hóa đơn không thành công ! Lỗi " + ex.Message, "Xóa chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaHD.Text = "";
            dtpNgayDH.Value = DateTime.Today;
            txtPTTT.Text = "";
            cboxMaNV.Text = string.Empty;
            cboxMaKH.Text = string.Empty;
            txtTienNhan.Text = "";
            txtTienThua.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string mahd = txtMaHD.Text;
                string ngaydh = dtpNgayDH.Value.ToString("MM/dd/yyyy");
                string pttt = txtPTTT.Text;
                string manv = cboxMaNV.Text;
                string makh = cboxMaKH.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm hóa đơn ?", "Thêm hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into HoaDon(MaHD, NgayDH, PTTT, MaNV, MaKH) "
                    + " values ( '" + mahd + "', '" + ngaydh + "', N'" + pttt + "', '" + manv + "', '" + makh + "')");
                    MessageBox.Show("Thêm hóa đơn thành công!", "Thêm hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm hóa đơn không thành công ! Lỗi " + ex.Message, "Thêm hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string mahd = txtMaHD.Text;
                string ngaydh = dtpNgayDH.Value.ToString("MM/dd/yyyy");
                string pttt = txtPTTT.Text;
                string manv = cboxMaNV.Text;
                string makh = cboxMaKH.Text;

                DialogResult result = MessageBox.Show("Bạn muốn sửa hóa đơn ?", "Sửa hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update HoaDon set NgayDH = '" + ngaydh
                        + "', PTTT = N'" + pttt + "', MaNV = '" + manv + "', MaKH = '" + makh + "' where MaHD = '" + mahd + "'");
                    MessageBox.Show("Sửa hóa đơn thành công!", "Sửa hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa hóa đơn không thành công ! Lỗi " + ex.Message, "Sửa hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string mahd = txtMaHD.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa hóa đơn ?", "Xóa hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from HoaDon where MaHD = '" + mahd + "'");
                    MessageBox.Show("Xóa hóa đơn thành công!", "Xóa hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa hóa đơn không thành công ! Bạn cần xóa chi tiết hóa đơn trước !", "Xóa hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string mahd = txt_timMaHD.Text;
            string ngaydh = dtp_timNgayDH.Value.ToString("MM/dd/yyyy");
            if (radMaHD.Checked && !radNgayDH.Checked)
            {
                if(mahd != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from HoaDon where MaHD = '" + mahd + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvHD.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvHD.Columns[4].Width = 110;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radMaHD.Checked && radNgayDH.Checked)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from HoaDon where NgayDH = '" + ngaydh + "'", data.GetConnect());
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    bdsource.DataSource = dt;
                    dgvHD.DataSource = bdsource;
                    txtHienHanh.Text = (bdsource.Position + 1).ToString();
                    lblTongTin.Text = bdsource.Count.ToString();
                    dgvHD.Columns[4].Width = 110;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnKoLoc_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTinh_Click(object sender, EventArgs e)
        {
            string texttong = txtTongTien.Text; // Lấy giá trị văn bản từ TextBox
            string numbertong = texttong.Replace(",", "").Replace(" VND", "");

            string textnhan = txtTienNhan.Text; // Lấy giá trị văn bản từ TextBox
            string numbernhan = textnhan.Replace(",", "").Replace(" VND", "");


            double tong = double.Parse(numbertong);
            double nhan = double.Parse(numbernhan);

            double thua = 0;
            if(tong > nhan)
            {
                MessageBox.Show("Số tiền nhận < Tổng hóa đơn! Không thể thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }    
            else
            {
                thua = Math.Abs(nhan - tong);
            }  
            txtTienNhan.Text = String.Format("{0:0,000 VND}", nhan);
            txtTienThua.Text = String.Format("{0:0,000 VND}", thua);
        }
    }
}
