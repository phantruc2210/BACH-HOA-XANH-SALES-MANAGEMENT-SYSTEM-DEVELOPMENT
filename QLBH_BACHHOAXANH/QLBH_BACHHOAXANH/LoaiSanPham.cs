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
    public partial class LoaiSanPham : Form
    {
        KetNoi data = new KetNoi();
        private BindingSource bdsource = new BindingSource();
        public LoaiSanPham()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            bdsource.DataSource = data.ThongTinLSP();
            dgvLSP.DataSource = bdsource;
            txtHienHanh.Text = (bdsource.Position + 1).ToString();
            lblTongTin.Text = bdsource.Count.ToString();
            // thay đổi độ rộng ô
            dgvLSP.Columns[0].Width = 150;
            dgvLSP.Columns[1].Width = 180;
            dgvLSP.Columns[2].Width = 150;
            // màu dòng
            dgvLSP.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvLSP.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvLSP.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLSP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void LoaiSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            txtMaLSP.Text = "";
            txtTenLSP.Text = "";
            txtGhiChu.Text = "";
        }

        private void dgvLSP_SelectionChanged(object sender, EventArgs e)
        {
            txtMaLSP.Text = dgvLSP.CurrentRow.Cells[0].Value.ToString();
            txtTenLSP.Text = dgvLSP.CurrentRow.Cells[1].Value.ToString();
            txtGhiChu.Text = dgvLSP.CurrentRow.Cells[2].Value.ToString();
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {

                string malsp = txtMaLSP.Text;
                string tenlsp = txtTenLSP.Text;
                string ghichu = txtGhiChu.Text;


                DialogResult result = MessageBox.Show("Bạn muốn thêm loại sản phẩm ?", "Thêm loại sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"insert into LoaiSanPham(MaloaiSP, TenloaiSP, GhiChu) values ('" + malsp + "', N'" + tenlsp + "', N'" + ghichu + "')");
                    MessageBox.Show("Thêm loại sản phẩm thành công!", "Thêm loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm loại sản phẩm không thành công ! Lỗi " + ex.Message, "Thêm loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {

                string malsp = txtMaLSP.Text;
                string tenlsp = txtTenLSP.Text;
                string ghichu = txtGhiChu.Text;


                DialogResult result = MessageBox.Show("Bạn muốn sửa loại sản phẩm ?", "Sửa loại sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"update LoaiSanPham set TenloaiSP = N'" + tenlsp + "', GhiChu = N'" + ghichu + "' where MaloaiSP = '"+ malsp + "'");
                    MessageBox.Show("Sửa loại sản phẩm thành công!", "Sửa loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa loại sản phẩm không thành công ! Lỗi " + ex.Message, "Sửa loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {

                string malsp = txtMaLSP.Text;

                DialogResult result = MessageBox.Show("Bạn muốn xóa loại sản phẩm ?", "Xóa loại sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.ExecuteNonQuery(@"delete from LoaiSanPham where MaloaiSP = '" + malsp + "'");
                    MessageBox.Show("Xóa loại sản phẩm thành công!", "Xóa loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa loại sản phẩm không thành công ! Lỗi " + ex.Message, "Xóa loại sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string malsp = txt_timMaLSP.Text;
            string tenlsp = txt_timTenLSP.Text;
            if (radMaLSP.Checked && !radTenLSP.Checked)   
            {
                if(malsp != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from LoaiSanPham where MaloaiSP = '" + malsp + "'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvLSP.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy loại sản phẩm ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (!radMaLSP.Checked && radTenLSP.Checked)
            {
                if(tenlsp != "")
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select * from LoaiSanPham where TenloaiSP like N'%" + tenlsp + "%'", data.GetConnect());
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bdsource.DataSource = dt;
                        dgvLSP.DataSource = bdsource;
                        txtHienHanh.Text = (bdsource.Position + 1).ToString();
                        lblTongTin.Text = bdsource.Count.ToString();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không tìm thấy loại sản phẩm ! " + ex.Message, "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
