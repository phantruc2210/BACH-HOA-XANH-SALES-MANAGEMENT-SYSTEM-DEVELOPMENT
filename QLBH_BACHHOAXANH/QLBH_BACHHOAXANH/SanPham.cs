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
    public partial class SanPham : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        private BindingSource bdsourceLSP = new BindingSource();
        private BindingSource bdsourceNCC = new BindingSource();

        public SanPham()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinSP();
            dgvSP.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // màu dòng
            dgvSP.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvSP.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvSP.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // thay đổi độ rộng ô
            dgvSP.Columns[0].Width = 60; 
            dgvSP.Columns[1].Width = 160;
            dgvSP.Columns[2].Width = 60;
            dgvSP.Columns[3].Width = 60;
            dgvSP.Columns[4].Width = 60;
            dgvSP.Columns[5].Width = 60;
            // nạp dữ liệu combo box đơn vị tính
            cboxDVT.Items.Clear();
            foreach (DataRowView row in bdsource)
            {
                string dvt = row["DonViTinh"].ToString();
                if (!cboxDVT.Items.Contains(dvt))
                {
                    cboxDVT.Items.Add(dvt);
                }
            }
            // nạp dữ liệu combo box mã loại sản phẩm
            bdsourceLSP.DataSource = data.ThongTinLSP();
            cboxMaLSP.Items.Clear();
            foreach (DataRowView row in bdsourceLSP)
            {
                string idlsp = row["MaloaiSP"].ToString();
                if (!cboxMaLSP.Items.Contains(idlsp))
                {
                    cboxMaLSP.Items.Add(idlsp);
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

        private void SanPham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvSP_SelectionChanged(object sender, EventArgs e)
        {
            txtMaSP.Text = dgvSP.CurrentRow.Cells[0].Value.ToString();
            txtTenSP.Text = dgvSP.CurrentRow.Cells[1].Value.ToString();
            txtSLTon.Text = dgvSP.CurrentRow.Cells[2].Value.ToString();
            cboxDVT.SelectedItem = dgvSP.CurrentRow.Cells["DonViTinh"].Value.ToString();
            cboxMaLSP.SelectedItem = dgvSP.CurrentRow.Cells["MaloaiSP"].Value.ToString();
            cboxMaNCC.SelectedItem = dgvSP.CurrentRow.Cells["MaNCC"].Value.ToString();
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtSLTon.Text = "";
            cboxDVT.Text = string.Empty;
            cboxMaLSP.Text = string.Empty;
            cboxMaNCC.Text = string.Empty;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string masp = txtMaSP.Text;
                string tensp = txtTenSP.Text;
                string sl = txtSLTon.Text;
                string dvt = cboxDVT.Text;
                string malsp = cboxMaLSP.Text;
                string mancc = cboxMaNCC.Text;

                DialogResult result = MessageBox.Show("Bạn muốn thêm sản phẩm ?", "Thêm sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into SanPham(MaSP, TenSP, SLTon, DonViTinh, MaloaiSP, MaNCC) values ('" 
                    + masp + "', N'" + tensp + "', '" + sl + "', N'" + dvt + "', '"+ malsp + "', '" + mancc + "')");
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm sản phẩm không thành công ! Lỗi " + ex.Message, "Thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string masp = txtMaSP.Text;
                string tensp = txtTenSP.Text;
                string sl = txtSLTon.Text;
                string dvt = cboxDVT.Text;
                string malsp = cboxMaLSP.Text;
                string mancc = cboxMaNCC.Text;

                DialogResult result = MessageBox.Show("Bạn muốn sửa sản phẩm ?", "Sửa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update SanPham set TenSP = N'"+ tensp + "', SLTon = '" + sl + "'," +
                        "DonViTinh = N'" + dvt + "', MaloaiSP = '" + malsp + "', MaNCC = '" + mancc + "' where MaSP = '"+ masp+"'");
                    MessageBox.Show("Sửa sản phẩm thành công!", "Sửa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa sản phẩm không thành công ! Lỗi " + ex.Message, "Sửa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string masp = txtMaSP.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa sản phẩm ?", "Xóa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from SanPham where MaSP = '" + masp + "'");
                    MessageBox.Show("Xóa sản phẩm thành công!", "Xóa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa sản phẩm không thành công ! Lỗi " + ex.Message, "Xóa sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string masp = txt_timMaSP.Text;
            string tensp = txt_timTenSP.Text;
            if (radMaSP.Checked && !radTenSP.Checked)
            {
                if(masp != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from SanPham where MaSP = '" + masp + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvSP.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvSP.Columns[5].Width = 75;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (!radMaSP.Checked && radTenSP.Checked)
            {
                if(tensp != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from SanPham where TenSP like N'%" + tensp + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvSP.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();
                        dgvSP.Columns[5].Width = 75;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm ! Lỗi " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
