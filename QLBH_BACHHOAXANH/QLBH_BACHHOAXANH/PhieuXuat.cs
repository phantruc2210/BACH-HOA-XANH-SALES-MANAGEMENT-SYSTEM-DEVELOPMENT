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
    public partial class PhieuXuat : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        private BindingSource bdsourceCTPX = new BindingSource();
        private BindingSource bdsourceNV = new BindingSource();
        private BindingSource bdsourceCH = new BindingSource();
        public PhieuXuat()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinPX();
            dgvPX.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // màu dòng
            dgvPX.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvPX.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvPX.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPX.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // thay đổi độ rộng ô
            dgvPX.Columns[0].Width = 100;
            dgvPX.Columns[1].Width = 100;
            dgvPX.Columns[2].Width = 100;
            dgvPX.Columns[3].Width = 100;
            dgvPX.Columns[4].Width = 100;
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
            // nạp dữ liệu combo box mã cửa hàng
            bdsourceCH.DataSource = data.ThongTinCH();
            cboxMaCH.Items.Clear();
            foreach (DataRowView row in bdsourceCH)
            {
                string idch = row["MaCH"].ToString();
                if (!cboxMaCH.Items.Contains(idch))
                {
                    cboxMaCH.Items.Add(idch);
                }

            }

        }

        private void PhieuXuat_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvPX_SelectionChanged(object sender, EventArgs e)
        {
            txtSoPX.Text = dgvPX.CurrentRow.Cells[0].Value.ToString();
            dtpNgayXuat.Value = (DateTime)dgvPX.CurrentRow.Cells[1].Value;
            txtGhiChu.Text = dgvPX.CurrentRow.Cells[2].Value.ToString();
            cboxMaNV.SelectedItem = dgvPX.CurrentRow.Cells["MaNV"].Value.ToString();
            cboxMaCH.SelectedItem = dgvPX.CurrentRow.Cells["MaCH"].Value.ToString();

            // HIỂN THỊ CHI TIẾT PHIẾU XUẤT
            bdsourceCTPX.DataSource = data.ThongTinCTPX(txtSoPX.Text);
            dgvCTPX.DataSource = bdsourceCTPX;
            dgvCTPX.Columns[0].Width = 125;
            dgvCTPX.Columns[1].Width = 125;
            dgvCTPX.Columns[2].Width = 125;
            dgvCTPX.Columns[3].Width = 125;
            // tính tổng tiền trên mỗi phiếu xuất
            double tong = data.TienTrenPX(txtSoPX.Text);
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

        private void btnThemCTPX_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPX.CurrentCell.RowIndex;
                string sopx = dgvCTPX.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPX.Rows[vt].Cells[1].Value.ToString();
                string sl = dgvCTPX.Rows[vt].Cells[2].Value.ToString();
                string dongia = dgvCTPX.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn thêm chi tiết phiếu xuất ?", "Thêm chi tiết phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into CTPhieuXuat(SoPX, MaSP, SoLuong, DonGia)" +
                        " values ('" + sopx + "', '" + masp + "', '" + sl + "' , '" + dongia + "')");
                    MessageBox.Show("Thêm chi tiết phiếu xuất thành công!", "Thêm chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm chi tiết phiếu xuất không thành công ! Lỗi " + ex.Message, "Thêm chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaCTPX_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPX.CurrentCell.RowIndex;
                string sopx = dgvCTPX.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPX.Rows[vt].Cells[1].Value.ToString();
                string sl = dgvCTPX.Rows[vt].Cells[2].Value.ToString();
                string dongia = dgvCTPX.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn sửa chi tiết phiếu xuất ?", "Sửa chi tiết phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update CTPhieuXuat set SoLuong = '" + sl + "' , DonGia = '" + dongia + "' where SoPX = '" + sopx + "' and MaSP = '" + masp + "'");
                    MessageBox.Show("Sửa chi tiết phiếu xuất thành công!", "Sửa chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa chi tiết phiếu xuất không thành công ! Lỗi " + ex.Message, "Sửa chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaCTPX_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPX.CurrentCell.RowIndex;
                string sopx = dgvCTPX.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPX.Rows[vt].Cells[1].Value.ToString();



                DialogResult result = MessageBox.Show("Bạn muốn xóa chi tiết phiếu xuất ?", "Xóa chi tiết phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from CTPhieuXuat where SoPX = '" + sopx + "' and MaSP = '" + masp + "'");
                    MessageBox.Show("Xóa chi tiết phiếu xuất thành công!", "Xóa chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa chi tiết phiếu xuất không thành công ! Lỗi " + ex.Message, "Xóa chi tiết phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string sopx = txtSoPX.Text;
                string ngayxuat = dtpNgayXuat.Value.ToString("MM/dd/yyyy");
                string ghichu = txtGhiChu.Text;
                string manv = cboxMaNV.Text;
                string mach = cboxMaCH.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm phiếu xuất ?", "Thêm phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into PhieuXuat(SoPX, NgayXuat, GhiChu, MaNV, MaCH) "
                    + " values ( '" + sopx + "', '" + ngayxuat + "', N'" + ghichu + "', '" + manv + "', '" + mach + "')");
                    MessageBox.Show("Thêm phiếu xuất thành công!", "Thêm phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm phiếu xuất không thành công ! Lỗi " + ex.Message, "Thêm phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string sopx = txtSoPX.Text;
                string ngayxuat = dtpNgayXuat.Value.ToString("MM/dd/yyyy");
                string ghichu = txtGhiChu.Text;
                string manv = cboxMaNV.Text;
                string mach = cboxMaCH.Text;

                DialogResult result = MessageBox.Show("Bạn muốn sửa phiếu xuất ?", "Sửa phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update PhieuXuat set NgayXuat = '" + ngayxuat + "', GhiChu = N'" + ghichu
                        + "', MaNV = '" + manv + "', MaCH = '" + mach + "' where SoPX = '" + sopx + "'");
                    MessageBox.Show("Sửa phiếu xuất thành công!", "Sửa phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa phiếu xuất không thành công ! Lỗi " + ex.Message, "Sửa phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string sopx = txtSoPX.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa phiếu xuất ?", "Xóa phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from PhieuXuat where SoPX = '" + sopx + "'");
                    MessageBox.Show("Xóa phiếu xuất thành công!", "Xóa phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa phiếu xuất không thành công ! Bạn cần xóa chi tiết phiếu xuất trước !", "Xóa phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtSoPX.Text = "";
            dtpNgayXuat.Value = DateTime.Today;
            txtGhiChu.Text = "";
            cboxMaNV.Text = string.Empty;
            cboxMaCH.Text = string.Empty;
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string sopx = txt_timSoPX.Text;
            string ngayxuat = dtp_timNgayXuat.Value.ToString("MM/dd/yyyy");
            if (radSoPX.Checked && !radNgayXuat.Checked)
            {
                if(sopx != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from PhieuXuat where SoPX = '" + sopx + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvPX.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvPX.Columns[4].Width = 105;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy phiếu xuất ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radSoPX.Checked && radNgayXuat.Checked)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from PhieuXuat where NgayXuat = '" + ngayxuat + "'", data.GetConnect());
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    bdsource.DataSource = dt;
                    dgvPX.DataSource = bdsource;
                    txtHienHanh.Text = (bdsource.Position + 1).ToString();
                    lblTongTin.Text = bdsource.Count.ToString();
                    dgvPX.Columns[4].Width = 105;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không tìm thấy phiếu xuất ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnKoLoc_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
