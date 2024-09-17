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
    public partial class NhaCungCap : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        public NhaCungCap()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinNCC();
            dgvNCC.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // thay đổi độ rộng ô
            dgvNCC.Columns[0].Width = 50;
            dgvNCC.Columns[4].Width = 150;

            // màu dòng
            dgvNCC.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvNCC.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvNCC.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtEmail.Text = "";
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

        private void dgvNCC_SelectionChanged(object sender, EventArgs e)
        {
            txtMaNCC.Text = dgvNCC.CurrentRow.Cells[0].Value.ToString();
            txtTenNCC.Text = dgvNCC.CurrentRow.Cells[1].Value.ToString();
            txtDiaChi.Text = dgvNCC.CurrentRow.Cells[2].Value.ToString();
            txtDienThoai.Text = dgvNCC.CurrentRow.Cells[3].Value.ToString();
            txtEmail.Text = dgvNCC.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string mancc = txtMaNCC.Text;
                string tenncc = txtTenNCC.Text;
                string diachi = txtDiaChi.Text;
                string dthoai = txtDienThoai.Text;
                string mail = txtEmail.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm nhà cung cấp ?", "Thêm nhà cung cấp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into NhaCungCap(MaNCC, TenNCC, DiaChi, DienThoai, Email) "
                    + " values ( '"+ mancc + "', N'" + tenncc + "', N'" + diachi + "', '"+ dthoai + "', '"+ mail+ "')");
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thêm nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm nhà cung cấp không thành công ! Lỗi " + ex.Message, "Thêm nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string mancc = txtMaNCC.Text;
                string tenncc = txtTenNCC.Text;
                string diachi = txtDiaChi.Text;
                string dthoai = txtDienThoai.Text;
                string mail = txtEmail.Text;


                DialogResult result = MessageBox.Show("Bạn muốn sửa nhà cung cấp ?", "Sửa nhà cung cấp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update NhaCungCap set TenNCC = N'" + tenncc + "', DiaChi = N'" + diachi + "', DienThoai = '" + dthoai + "', Email = '" + mail + "' " +
                        "where MaNCC = '"+ mancc + "'");
                    MessageBox.Show("Sửa nhà cung cấp thành công!", "Sửa nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa nhà cung cấp không thành công ! Lỗi " + ex.Message, "Sửa nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string mancc = txtMaNCC.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa nhà cung cấp ?", "Xóa nhà cung cấp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from NhaCungCap where MaNCC = '" + mancc + "'");
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Xóa nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa nhà cung cấp không thành công ! Lỗi " + ex.Message, "Xóa nhà cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string mancc = txt_timMaNCC.Text;
            string tenncc = txt_timTenNCC.Text;
            if (radMaNCC.Checked && !radTenNCC.Checked)
            {
                if(mancc != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from NhaCungCap where MaNCC = '" + mancc + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvNCC.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radMaNCC.Checked && radTenNCC.Checked)
            {
                if(tenncc != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from NhaCungCap where TenNCC like N'%" + tenncc + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvNCC.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
