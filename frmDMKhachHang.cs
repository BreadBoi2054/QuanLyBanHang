using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyBanHang.Class; //Sử dụng lớp Functions
using System.Data.SqlClient; //Sử dụng lớp SqlConnection, SqlCommand, SqlDataAdapter

namespace QuanLyBanHang
{
    public partial class frmDMKhachHang: Form
    {
        private DataTable tblKH; //Khai báo biến tblKhachHang kiểu DataTable
        public frmDMKhachHang()
        {
            InitializeComponent();
        }

        private void frmDMKhachHang_Load(object sender, EventArgs e)
        {
            txtMaKhach.Enabled = false; //Không cho phép sửa mã khách hàng
            btnLuu.Enabled = false; //Không cho phép lưu
            btnBoqua.Enabled = false; //Không cho phép bỏ qua
            LoadDataGridView(); //Gọi hàm LoadDataGridView
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaKhach, TenKhach, DiaChi, DienThoai FROM tblKhach"; //Câu lệnh SQL
            tblKH = Functions.GetDataToTable(sql); //Lấy dữ liệu từ bảng tblKhach
            dgvKhachHang.DataSource = tblKH; //Gán dữ liệu cho DataGridView
            dgvKhachHang.Columns[0].HeaderText = "Mã khách hàng"; //Đặt tiêu đề cột
            dgvKhachHang.Columns[1].HeaderText = "Tên khách hàng"; //Đặt tiêu đề cột
            dgvKhachHang.Columns[2].HeaderText = "Địa chỉ"; //Đặt tiêu đề cột
            dgvKhachHang.Columns[3].HeaderText = "Điện thoại"; //Đặt tiêu đề cột
            dgvKhachHang.Columns[0].Width = 150; //Đặt chiều rộng cột
            dgvKhachHang.Columns[1].Width = 200; //Đặt chiều rộng cột
            dgvKhachHang.Columns[2].Width = 200; //Đặt chiều rộng cột
            dgvKhachHang.Columns[3].Width = 150; //Đặt chiều rộng cột
            dgvKhachHang.AllowUserToAddRows = false; //Không cho phép thêm dòng
            dgvKhachHang.EditMode = DataGridViewEditMode.EditProgrammatically; //Chế độ chỉnh sửa
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close(); //Đóng form
        }

        private void ResetValues()
        {
            txtMaKhach.Text = ""; //Xóa mã khách hàng
            txtTenKhach.Text = ""; //Xóa tên khách hàng
            txtDiaChi.Text = ""; //Xóa địa chỉ
            mtbDienThoai.Text = ""; //Xóa điện thoại
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            txtMaKhach.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaKhach.Text.Trim().Length == 0) //Kiểm tra mã khách hàng
            {
                MessageBox.Show("Bạn phải nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhach.Focus(); //Đưa con trỏ vào ô mã khách hàng
                return;
            }
            if (txtTenKhach.Text.Trim().Length == 0) //Kiểm tra tên khách hàng
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhach.Focus(); //Đưa con trỏ vào ô tên khách hàng
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0) //Kiểm tra địa chỉ
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus(); //Đưa con trỏ vào ô địa chỉ
                return;
            }
            if (mtbDienThoai.Text.Trim().Length == 0) //Kiểm tra điện thoại
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus(); //Đưa con trỏ vào ô điện thoại
                return;
            }
            sql = "SELECT MaKhach FROM tblKhach WHERE MaKhach=N'" + txtMaKhach.Text.Trim() + "'"; //Câu lệnh SQL
            if (Functions.CheckKey(sql)) //Kiểm tra mã khách hàng đã tồn tại chưa
            {
                MessageBox.Show("Mã khách hàng đã tồn tại, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhach.Focus(); //Đưa con trỏ vào ô mã khách hàng
                txtMaKhach.Text = ""; //Xóa mã khách hàng
                return;
            }
            sql = "INSERT INTO tblKhach(MaKhach, TenKhach, DiaChi, DienThoai) VALUES(N'" + txtMaKhach.Text.Trim() + "', N'" + txtTenKhach.Text.Trim() + "', N'" + txtDiaChi.Text.Trim() + "', '" + mtbDienThoai.Text.Trim() + "')"; //Câu lệnh SQL
            Functions.RunSQL(sql); //Thực hiện câu lệnh SQL
            LoadDataGridView(); //Gọi hàm LoadDataGridView
            ResetValues(); //Gọi hàm ResetValues
            btnLuu.Enabled = false; //Không cho phép lưu
            btnBoqua.Enabled = false; //Không cho phép bỏ qua
            btnThem.Enabled = true; //Cho phép thêm
            btnXoa.Enabled = true; //Cho phép xóa
            btnSua.Enabled = true; //Cho phép sửa
            txtMaKhach.Enabled = false; //Không cho phép sửa mã khách hàng
            txtTenKhach.Enabled = false;
            txtDiaChi.Enabled = false; //Không cho phép sửa địa chỉ
            mtbDienThoai.Enabled = false; //Không cho phép sửa điện thoại
        }

        private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false) 
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tblKH.Rows.Count == 0) //Kiểm tra bảng tblKhach có dữ liệu không
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int r = dgvKhachHang.CurrentCell.RowIndex; //Lấy chỉ số dòng hiện tại
            txtMaKhach.Text = dgvKhachHang.Rows[r].Cells[0].Value.ToString(); //Gán giá trị ô mã khách hàng
            txtTenKhach.Text = dgvKhachHang.Rows[r].Cells[1].Value.ToString(); //Gán giá trị ô tên khách hàng
            txtDiaChi.Text = dgvKhachHang.Rows[r].Cells[2].Value.ToString(); //Gán giá trị ô địa chỉ
            mtbDienThoai.Text = dgvKhachHang.Rows[r].Cells[3].Value.ToString(); //Gán giá trị ô điện thoại
            btnSua.Enabled = true; //Cho phép sửa
            btnXoa.Enabled = true; //Cho phép xóa
            btnThem.Enabled = true; //Cho phép thêm
            btnLuu.Enabled = false; //Không cho phép lưu
            btnBoqua.Enabled = false; //Không cho phép bỏ qua
            txtMaKhach.Enabled = false; //Không cho phép sửa mã khách hàng 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues(); //Gọi hàm ResetValues
            btnLuu.Enabled = true; //Cho phép lưu
            btnBoqua.Enabled = true; //Cho phép bỏ qua
            btnThem.Enabled = false; //Không cho phép thêm
            btnXoa.Enabled = false; //Không cho phép xóa
            btnSua.Enabled = false; //Không cho phép sửa
            txtMaKhach.Enabled = true; //Cho phép sửa mã khách hàng
            txtTenKhach.Enabled = true; //Cho phép sửa tên khách hàng
            txtDiaChi.Enabled = true; //Cho phép sửa địa chỉ
            mtbDienThoai.Enabled = true; //Cho phép sửa điện thoại
            txtMaKhach.Focus(); //Đưa con trỏ vào ô mã khách hàng
            txtMaKhach.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblKH.Rows.Count == 0) //Kiểm tra bảng tblKhach có dữ liệu không
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMaKhach.Text == "") //Kiểm tra mã khách hàng
            {
                MessageBox.Show("Bạn phải chọn mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) //Hỏi người dùng có muốn xóa không
            {
                sql = "DELETE tblKhach WHERE MaKhach=N'" + txtMaKhach.Text + "'"; //Câu lệnh SQL
                Functions.RunSQL(sql); //Thực hiện câu lệnh SQL
                LoadDataGridView(); //Gọi hàm LoadDataGridView
                ResetValues(); //Gọi hàm ResetValues
            }
            if (tblKH.Rows.Count == 0) //Kiểm tra bảng tblKhach có dữ liệu không
            {
                btnXoa.Enabled = false; //Không cho phép xóa
                btnSua.Enabled = false; //Không cho phép sửa
            }
            else
            {
                btnXoa.Enabled = true; //Cho phép xóa
                btnSua.Enabled = true; //Cho phép sửa
            }
            btnLuu.Enabled = false; //Không cho phép lưu
            btnBoqua.Enabled = false; //Không cho phép bỏ qua
            btnThem.Enabled = true; //Cho phép thêm
            btnXoa.Enabled = true; //Cho phép xóa
            btnSua.Enabled = true; //Cho phép sửa
            txtMaKhach.Enabled = false; //Không cho phép sửa mã khách hàng
            txtTenKhach.Enabled = false; //Không cho phép sửa tên khách hàng
            txtDiaChi.Enabled = false; //Không cho phép sửa địa chỉ
            mtbDienThoai.Enabled = false; //Không cho phép sửa điện thoại
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblKH.Rows.Count == 0) //Kiểm tra bảng tblKhach có dữ liệu không
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMaKhach.Text == "") //Kiểm tra mã khách hàng
            {
                MessageBox.Show("Bạn phải chọn mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtTenKhach.Text.Trim().Length == 0) //Kiểm tra tên khách hàng
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhach.Focus(); //Đưa con trỏ vào ô tên khách hàng
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0) //Kiểm tra địa chỉ
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus(); //Đưa con trỏ vào ô địa chỉ
                return;
            }
            if (mtbDienThoai.Text.Trim().Length == 0) //Kiểm tra điện thoại
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus(); //Đưa con trỏ vào ô điện thoại
                return;
            }
            sql = "UPDATE tblKhach SET TenKhach=N'" + txtTenKhach.Text.Trim() + "', DiaChi=N'" + txtDiaChi.Text.Trim() + "', DienThoai='" + mtbDienThoai.Text.Trim() + "' WHERE MaKhach=N'" + txtMaKhach.Text.Trim() + "'"; //Câu lệnh SQL
            Functions.RunSQL(sql); //Thực hiện câu lệnh SQL
            LoadDataGridView(); //Gọi hàm LoadDataGridView
            ResetValues(); //Gọi hàm ResetValues
            btnLuu.Enabled = false; //Không cho phép lưu
            btnBoqua.Enabled = false; //Không cho phép bỏ qua
            btnThem.Enabled = true; //Cho phép thêm
            btnXoa.Enabled = true; //Cho phép xóa
            btnSua.Enabled = true; //Cho phép sửa
            txtMaKhach.Enabled = false; //Không cho phép sửa mã khách hàng
        }
    }
}
