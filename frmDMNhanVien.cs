using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyBanHang.Class;

namespace QuanLyBanHang
{
    public partial class frmDMNhanVien : Form
    {
        private DataTable tblNV;
        public frmDMNhanVien()
        {
            InitializeComponent();
        }

        private void frmDMNhanVien_Load(object sender, EventArgs e)
        {
            txtManhanvien.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            LoadDataGridView();
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT Manhanvien, Tennhanvien, Gioitinh, Diachi, Dienthoai, Ngaysinh FROM tblNhanVien";
            tblNV = Functions.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            dgvNhanVien.DataSource = tblNV;
            dgvNhanVien.Columns[0].HeaderText = "Mã nhân viên";
            dgvNhanVien.Columns[1].HeaderText = "Tên nhân viên";
            dgvNhanVien.Columns[2].HeaderText = "Giới tính";
            dgvNhanVien.Columns[3].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns[4].HeaderText = "Điện thoại";
            dgvNhanVien.Columns[5].HeaderText = "Ngày sinh";
            dgvNhanVien.Columns[0].Width = 100;
            dgvNhanVien.Columns[1].Width = 150;
            dgvNhanVien.Columns[2].Width = 100;
            dgvNhanVien.Columns[3].Width = 150;
            dgvNhanVien.Columns[4].Width = 100;
            dgvNhanVien.Columns[5].Width = 100;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtManhanvien.Focus();
                return;
            }
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtManhanvien.Text = dgvNhanVien.CurrentRow.Cells["Manhanvien"].Value.ToString();
            txtTennhanvien.Text = dgvNhanVien.CurrentRow.Cells["Tennhanvien"].Value.ToString();
            if (dgvNhanVien.CurrentRow.Cells["Gioitinh"].Value.ToString() == "Nam")
                chkGioitinh.Checked = true;
            else
                chkGioitinh.Checked = false;
            txtDiachi.Text = dgvNhanVien.CurrentRow.Cells["Diachi"].Value.ToString();
            mtbDienthoai.Text = dgvNhanVien.CurrentRow.Cells["Dienthoai"].Value.ToString();
            dtpNgaysinh.Value = DateTime.Parse(dgvNhanVien.CurrentRow.Cells["Ngaysinh"].Value.ToString());
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnBoqua.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            ResetValues();
            txtManhanvien.Enabled = true;
            txtManhanvien.Focus();
        }

        private void ResetValues()
        {
            txtManhanvien.Text = "";
            txtTennhanvien.Text = "";
            chkGioitinh.Checked = false;
            txtDiachi.Text = "";
            mtbDienthoai.Text = "";
            dtpNgaysinh.Value = DateTime.Now;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql,gt;
            if (txtManhanvien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtManhanvien.Focus();
                return;
            }
            if (txtTennhanvien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTennhanvien.Focus();
                return;
            }
            if (txtDiachi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }
            if (mtbDienthoai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienthoai.Focus();
                return;
            }
            if (chkGioitinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "SELECT Manhanvien FROM tblNhanVien WHERE Manhanvien=N'" + txtManhanvien.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhân viên này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtManhanvien.Focus();
                txtManhanvien.Text = "";
                return;
            }
            //INSERT INTO tblNhanVien(Manhanvien, Tennhanvien, Gioitinh, Diachi, Dienthoai, Ngaysinh) VALUES (N'MNV001', N'Trần Văn A', N'Nam', N'Đà Nẵng', '0123456789', '1990-01-01')
            sql = "INSERT INTO tblNhanVien(Manhanvien, Tennhanvien, Gioitinh, Diachi, Dienthoai, Ngaysinh) VALUES (N'" + txtManhanvien.Text.Trim() + "', N'" + txtTennhanvien.Text.Trim() + "', N'" + gt + "', N'" + txtDiachi.Text.Trim() + "', '" + mtbDienthoai.Text.Trim() + "', '" + dtpNgaysinh.Value.ToString("yyyy-MM-dd") + "')";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            txtManhanvien.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql, gt;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtManhanvien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn bản ghi cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtTennhanvien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTennhanvien.Focus();
                return;
            }
            if (txtDiachi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }
            if (mtbDienthoai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienthoai.Focus();
                return;
            }
            if (chkGioitinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "SELECT Manhanvien FROM tblNhanVien WHERE Manhanvien=N'" + txtManhanvien.Text.Trim() + "'";
            if (!Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhân viên này không có trong cơ sở dữ liệu, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtManhanvien.Focus();
                txtManhanvien.Text = "";
                return;
            }
            sql = "UPDATE tblNhanVien SET Tennhanvien=N'" + txtTennhanvien.Text.Trim() + "', Gioitinh=N'" + gt + "', Diachi=N'" + txtDiachi.Text.Trim() + "', Dienthoai='" + mtbDienthoai.Text.Trim() + "', Ngaysinh='" + dtpNgaysinh.Value.ToString("yyyy-MM-dd") + "' WHERE Manhanvien=N'" + txtManhanvien.Text.Trim() + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            txtManhanvien.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtManhanvien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn bản ghi cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có thực sự muốn xóa nhân viên này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblNhanVien WHERE Manhanvien=N'" + txtManhanvien.Text.Trim() + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
            }
            if (tblNV.Rows.Count == 0)
            {
                btnXoa.Enabled = false;
            }
            else
            {
                btnXoa.Enabled = true;
            }
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            txtManhanvien.Enabled = false;
            txtManhanvien.Enabled = true;
            txtManhanvien.Focus();
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            txtManhanvien.Enabled = false;
        }



        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();        
        }

        private void txtManhanvien_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}"); //Chuyển focus sang textbox tiếp theo
            }
        }
    }
}
