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
    public partial class NhanVien : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        private BindingSource bdsourceCH = new BindingSource();

        public NhanVien()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinNV();
            dgvNhanVien.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // thay đổi độ rộng ô
            dgvNhanVien.Columns[0].Width = 50;
            dgvNhanVien.Columns[1].Width = 50;
            dgvNhanVien.Columns[3].Width = 50;
            dgvNhanVien.Columns[4].Width = 70;
            dgvNhanVien.Columns[8].Width = 70;
            dgvNhanVien.Columns[9].Width = 40;
            dgvNhanVien.Columns[10].Width = 50;
            // màu dòng
            dgvNhanVien.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvNhanVien.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvNhanVien.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

            // nạp dữ liệu combo box giới tính
            cboxGioiTinh.Items.Clear();
            foreach (DataRowView row in bdsource)
            {
                string gender = row["GioiTinh"].ToString();
                if (!cboxGioiTinh.Items.Contains(gender))
                {
                    cboxGioiTinh.Items.Add(gender);
                }
            }

        }



        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();

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

        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            txtMaNV.Text = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();
            txtHoNV.Text = dgvNhanVien.CurrentRow.Cells[1].Value.ToString();
            txtTenNV.Text = dgvNhanVien.CurrentRow.Cells[2].Value.ToString();
            cboxGioiTinh.SelectedItem = dgvNhanVien.CurrentRow.Cells["GioiTinh"].Value.ToString();
            dtpNgaySinh.Value = (DateTime)dgvNhanVien.CurrentRow.Cells[4].Value;
            txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells[5].Value.ToString();
            txtDienThoai.Text = dgvNhanVien.CurrentRow.Cells[6].Value.ToString();
            txtNoiSinh.Text = dgvNhanVien.CurrentRow.Cells[7].Value.ToString();
            dtpNgayVaoLam.Value = (DateTime)dgvNhanVien.CurrentRow.Cells[8].Value;
            txtEmail.Text = dgvNhanVien.CurrentRow.Cells[9].Value.ToString();
            cboxMaCH.SelectedItem = dgvNhanVien.CurrentRow.Cells["MaCH"].Value.ToString();

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaNV.Text = "";
            txtHoNV.Text = "";
            txtTenNV.Text = "";
            cboxGioiTinh.Text = string.Empty;
            dtpNgaySinh.Value = DateTime.Today;
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtNoiSinh.Text = "";
            dtpNgayVaoLam.Value = DateTime.Today;
            txtEmail.Text = "";
            cboxMaCH.Text = string.Empty;
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string manv = txt_timMaNV.Text;
            string tennv = txt_timTenNV.Text;
            if (radMaNV.Checked && !radTenNV.Checked)
            {
                if(manv != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from NhanVien where MaNV = '" + manv + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvNhanVien.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvNhanVien.Columns[10].Width = 65;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radMaNV.Checked && radTenNV.Checked)
            {
                if(tennv != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from NhanVien where TenNV like N'%" + tennv + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvNhanVien.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvNhanVien.Columns[10].Width = 65;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string manv = txtMaNV.Text;
                string honv = txtHoNV.Text;
                string tennv = txtTenNV.Text;
                string gt = cboxGioiTinh.Text;
                string nsinh = dtpNgaySinh.Value.ToString("MM/dd/yyyy");
                string diachi = txtDiaChi.Text;
                string dthoai = txtDienThoai.Text;
                string noisinh = txtNoiSinh.Text;
                string nvl = dtpNgayVaoLam.Value.ToString("MM/dd/yyyy");
                string mail = txtEmail.Text;
                string mach = cboxMaCH.Text;
               
                DialogResult result = MessageBox.Show("Bạn muốn thêm nhân viên ?", "Thêm nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into NhanVien(MaNV, HoNV, TenNV, GioiTinh, NgaySinh, DiaChi, DienThoai, NoiSinh, NgayVaoLam, Email, MaCH)"
                    + " values ('" + manv + "', N'"+ honv + "', N'"+ tennv + "', N'"+gt+"' , '"+ nsinh + "', N'"+ diachi + "', '"+ dthoai + "', N'" + noisinh + "', '"+ nvl
                    + "', '"+ mail + "', '" + mach +"')");
                    MessageBox.Show("Thêm nhân viên thành công!", "Thêm nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm nhân viên không thành công ! Lỗi " + ex.Message, "Thêm nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string manv = txtMaNV.Text;
                string honv = txtHoNV.Text;
                string tennv = txtTenNV.Text;
                string gt = cboxGioiTinh.Text;
                string nsinh = dtpNgaySinh.Value.ToString("MM/dd/yyyy");
                string diachi = txtDiaChi.Text;
                string dthoai = txtDienThoai.Text;
                string noisinh = txtNoiSinh.Text;
                string nvl = dtpNgayVaoLam.Value.ToString("MM/dd/yyyy");
                string mail = txtEmail.Text;
                string mach = cboxMaCH.Text;

                DialogResult result = MessageBox.Show("Bạn muốn sửa nhân viên ?", "Sửa nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update NhanVien set HoNV = N'" + honv + "', TenNV = N'" + tennv + "', GioiTinh = N'" + gt + "' , NgaySinh = '" + nsinh + 
                        "', DiaChi = N'" + diachi + "', DienThoai = '" + dthoai + "', NoiSinh = N'" + noisinh + "', NgayVaoLam = '" + nvl + "', Email = '" + mail + "', MaCH = '" + mach + "' " +
                        " where MaNV = '"+ manv +"'");
                    MessageBox.Show("Sửa nhân viên thành công!", "Sửa nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa nhân viên không thành công ! Lỗi " + ex.Message, "Sửa nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string manv = txtMaNV.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa nhân viên ?", "Xóa nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from NhanVien where MaNV = '"+ manv + "'");
                    MessageBox.Show("Xóa nhân viên thành công!", "Xóa nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa nhân viên không thành công ! Lỗi " + ex.Message, "Xóa nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
