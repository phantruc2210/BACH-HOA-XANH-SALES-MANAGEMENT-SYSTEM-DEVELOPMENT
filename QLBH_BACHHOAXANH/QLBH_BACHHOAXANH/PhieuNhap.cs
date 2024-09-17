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
    public partial class PhieuNhap : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        private BindingSource bdsourceCTPN = new BindingSource();
        private BindingSource bdsourceNV = new BindingSource();
        private BindingSource bdsourceNCC = new BindingSource();

        public PhieuNhap()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinPN();
            dgvPN.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // màu dòng
            dgvPN.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvPN.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvPN.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // thay đổi độ rộng ô
            dgvPN.Columns[0].Width = 70;
            dgvPN.Columns[1].Width = 70;
            dgvPN.Columns[2].Width = 70;
            dgvPN.Columns[3].Width = 125;
            dgvPN.Columns[4].Width = 70;
            dgvPN.Columns[5].Width = 70;
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
            // nạp dữ liệu combo box mã nhà cung cấp
            bdsourceNCC.DataSource = data.ThongTinNCC();
            cboxMaNCC.Items.Clear();
            foreach (DataRowView row in bdsourceNCC)
            {
                string idncc = row["MaNCC"].ToString();
                if (!cboxMaNCC.Items.Contains(idncc))
                {
                    cboxMaNCC.Items.Add(idncc);
                }

            }

        }

        private void PhieuNhap_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvPN_SelectionChanged(object sender, EventArgs e)
        {
            txtSoPN.Text = dgvPN.CurrentRow.Cells[0].Value.ToString();
            dtpNgayNhap.Value = (DateTime)dgvPN.CurrentRow.Cells[1].Value;
            txtGhiChu.Text = dgvPN.CurrentRow.Cells[2].Value.ToString();
            txtPTTT.Text = dgvPN.CurrentRow.Cells[3].Value.ToString();
            cboxMaNV.SelectedItem = dgvPN.CurrentRow.Cells["MaNV"].Value.ToString();
            cboxMaNCC.SelectedItem = dgvPN.CurrentRow.Cells["MaNCC"].Value.ToString();

            // HIỂN THỊ CHI TIẾT PHIỂU NHẬP
            bdsourceCTPN.DataSource = data.ThongTinCTPN(txtSoPN.Text);
            dgvCTPN.DataSource = bdsourceCTPN;
            dgvCTPN.Columns[0].Width = 120;
            dgvCTPN.Columns[1].Width = 125;
            dgvCTPN.Columns[2].Width = 125;
            dgvCTPN.Columns[3].Width = 125;
            // tính tổng tiền trên mỗi phiếu nhập
            double tong = data.TienTrenPN(txtSoPN.Text);
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

        private void btnThemCTPN_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPN.CurrentCell.RowIndex;
                string sopn = dgvCTPN.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPN.Rows[vt].Cells[1].Value.ToString();
                string sl = dgvCTPN.Rows[vt].Cells[2].Value.ToString();
                string dongia = dgvCTPN.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn thêm chi tiết phiếu nhập ?", "Thêm chi tiết phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into CTPhieuNhap(SoPN, MaSP, SoLuong, DonGia)" +
                        " values ('" + sopn + "', '" + masp + "', '" + sl + "' , '" + dongia + "')");
                    MessageBox.Show("Thêm chi tiết phiếu nhập thành công!", "Thêm chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm chi tiết phiếu nhập không thành công ! Lỗi " + ex.Message, "Thêm chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaCTPN_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPN.CurrentCell.RowIndex;
                string sopn = dgvCTPN.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPN.Rows[vt].Cells[1].Value.ToString();
                string sl = dgvCTPN.Rows[vt].Cells[2].Value.ToString();
                string dongia = dgvCTPN.Rows[vt].Cells[3].Value.ToString();


                DialogResult result = MessageBox.Show("Bạn muốn sửa chi tiết phiếu nhập ?", "Sửa chi tiết phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update CTPhieuNhap set SoLuong = '" + sl + "' , DonGia = '" + dongia + "' where SoPN = '"+ sopn + "' and MaSP = '"+ masp +"'");
                    MessageBox.Show("Sửa chi tiết phiếu nhập thành công!", "Sửa chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa chi tiết phiếu nhập không thành công ! Lỗi " + ex.Message, "Sửa chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaCTPN_Click(object sender, EventArgs e)
        {
            try
            {
                int vt = dgvCTPN.CurrentCell.RowIndex;
                string sopn = dgvCTPN.Rows[vt].Cells[0].Value.ToString();
                string masp = dgvCTPN.Rows[vt].Cells[1].Value.ToString();
                


                DialogResult result = MessageBox.Show("Bạn muốn xóa chi tiết phiếu nhập ?", "Xóa chi tiết phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from CTPhieuNhap where SoPN = '" + sopn + "' and MaSP = '" + masp + "'");
                    MessageBox.Show("Xóa chi tiết phiếu nhập thành công!", "Xóa chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa chi tiết phiếu nhập không thành công ! Lỗi " + ex.Message, "Xóa chi tiết phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string sopn = txtSoPN.Text;
                string ngaynhap = dtpNgayNhap.Value.ToString("MM/dd/yyyy");
                string ghichu = txtGhiChu.Text;
                string pttt = txtPTTT.Text;
                string manv = cboxMaNV.Text;
                string mancc = cboxMaNCC.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm phiếu nhập ?", "Thêm phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into PhieuNhap(SoPN, NgayNhap, GhiChu, PTTT, MaNV, MaNCC) "
                    + " values ( '" + sopn + "', '" + ngaynhap + "', N'" + ghichu + "', N'" + pttt + "', '" + manv + "', '" + mancc + "')");
                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thêm phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm phiếu nhập không thành công ! Lỗi " + ex.Message, "Thêm phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string sopn = txtSoPN.Text;
                string ngaynhap = dtpNgayNhap.Value.ToString("MM/dd/yyyy");
                string ghichu = txtGhiChu.Text;
                string pttt = txtPTTT.Text;
                string manv = cboxMaNV.Text;
                string mancc = cboxMaNCC.Text;


                DialogResult result = MessageBox.Show("Bạn muốn sửa phiếu nhập ?", "Sửa phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update PhieuNhap set NgayNhap = '" + ngaynhap + "', GhiChu = N'" + ghichu 
                        + "', PTTT = N'" + pttt + "', MaNV = '" + manv + "', MaNCC = '" + mancc + "' where SoPN = '" + sopn + "'");
                    MessageBox.Show("Sửa phiếu nhập thành công!", "Sửa phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa phiếu nhập không thành công ! Lỗi " + ex.Message, "Sửa phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string sopn = txtSoPN.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa phiếu nhập ?", "Xóa phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from PhieuNhap where SoPN = '" + sopn + "'");
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Xóa phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa phiếu nhập không thành công ! Bạn cần xóa chi tiết phiếu nhập trước !", "Xóa phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string sopn = txt_timSoPN.Text;
            string ngaynhap = dtp_timNgayNhap.Value.ToString("MM/dd/yyyy");
            if (radSoPN.Checked && !radNgayNhap.Checked)
            {
                if(sopn != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from PhieuNhap where SoPN = '" + sopn + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvPN.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvPN.Columns[5].Width = 100;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy phiếu nhập ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radSoPN.Checked && radNgayNhap.Checked)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from PhieuNhap where NgayNhap = '" + ngaynhap + "'", data.GetConnect());
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    bdsource.DataSource = dt;
                    dgvPN.DataSource = bdsource;
                    txtHienHanh.Text = (bdsource.Position + 1).ToString();
                    lblTongTin.Text = bdsource.Count.ToString();
                    dgvPN.Columns[5].Width = 100;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không tìm thấy phiếu nhập ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtSoPN.Text = "";
            dtpNgayNhap.Value = DateTime.Today;
            txtGhiChu.Text = "";
            txtPTTT.Text = "";
            cboxMaNV.Text = string.Empty;
            cboxMaNCC.Text = string.Empty;

        }

        private void btnKoLoc_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
