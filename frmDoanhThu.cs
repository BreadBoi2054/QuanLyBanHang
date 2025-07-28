using QuanLyBanHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyBanHang
{
    public partial class frmDoanhThu : Form
    {
        DataTable tblDoanhThu; //Bảng chi tiết hoá đơn bán
        public frmDoanhThu()
        {
            InitializeComponent();
        }
        private void LoadDataGridView()
        {
            string sql;
            // Lấy dữ liệu từ bảng tblChiTietHDBan và tblHDBan để hiển thị trong DataGridView
            sql = "SELECT a.MaHDBan, c.TenHang, a.SoLuong, a.DonGia, a.GiamGia, b.NgayBan, b.TongTien " +
                  "FROM tblChiTietHDBan AS a, tblHDBan AS b, tblHang AS c " +
                  "WHERE a.MaHDBan = b.MaHDBan AND a.MaHang = c.MaHang " +
                  "ORDER BY a.MaHDBan"; //Sắp xếp theo mã hóa đơn bán
            tblDoanhThu = Functions.GetDataToTable(sql);
            dgvDoanhThu.DataSource = tblDoanhThu;
            dgvDoanhThu.Columns[0].HeaderText = "Mã hóa đơn bán";
            dgvDoanhThu.Columns[1].HeaderText = "Tên hàng";
            dgvDoanhThu.Columns[2].HeaderText = "Số lượng";
            dgvDoanhThu.Columns[3].HeaderText = "Đơn giá";
            dgvDoanhThu.Columns[4].HeaderText = "Giảm giá %";
            dgvDoanhThu.Columns[5].HeaderText = "Ngày bán";
            dgvDoanhThu.Columns[6].HeaderText = "Tổng tiền";
            dgvDoanhThu.Columns[0].Width = 130;
            dgvDoanhThu.Columns[1].Width = 130;
            dgvDoanhThu.Columns[2].Width = 80;
            dgvDoanhThu.Columns[3].Width = 90;
            dgvDoanhThu.Columns[4].Width = 90;
            dgvDoanhThu.Columns[5].Width = 90;
            dgvDoanhThu.Columns[6].Width = 90;
            dgvDoanhThu.AllowUserToAddRows = false;
            dgvDoanhThu.EditMode = DataGridViewEditMode.EditProgrammatically;

        }

        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            LoadDataGridView(); //Gọi hàm LoadDataGridView để nạp dữ liệu vào DataGridView
            LoadChartDoanhThu(); //Gọi hàm loadchartDoanhThu để nạp dữ liệu vào biểu đồ doanh thu
            LoadDateTimePicker(); //Gọi hàm LoadDateTimePicker để nạp dữ liệu vào DateTimePicker
        }
        private void LoadChartDoanhThu()
        {
            // Xoá series "Series1" nếu nó tồn tại
            if (chartDoanhThu.Series.IndexOf("Series1") >= 0)
            {
                // Nếu muốn giữ lại dữ liệu, có thể đổi tên "Series1" thành "Doanh thu"
                chartDoanhThu.Series["Series1"].Name = "Doanh thu";
            }

            // Tạo hoặc lấy series "Doanh thu"
            var doanhThuSeries = chartDoanhThu.Series.FirstOrDefault(s => s.Name == "Doanh thu")
                ?? chartDoanhThu.Series.Add("Doanh thu");
            doanhThuSeries.ChartType = SeriesChartType.Column;
            doanhThuSeries.Points.Clear();

            // Tạo dictionary để cộng dồn doanh thu theo ngày
            var doanhThuTheoNgay = new SortedDictionary<DateTime, double>();

            foreach (DataGridViewRow row in dgvDoanhThu.Rows)
            {
                if (row.IsNewRow) continue;
                var ngayBan = Convert.ToDateTime(row.Cells["NgayBan"].Value);
                var tongTien = Convert.ToDouble(row.Cells["TongTien"].Value);

                if (doanhThuTheoNgay.ContainsKey(ngayBan))
                    doanhThuTheoNgay[ngayBan] += tongTien;
                else
                    doanhThuTheoNgay[ngayBan] = tongTien;
            }

            // Thêm dữ liệu vào series theo thứ tự ngày tăng dần
            foreach (var kvp in doanhThuTheoNgay)
            {
                doanhThuSeries.Points.AddXY(kvp.Key.ToString("dd/MM/yyyy"), kvp.Value);
            }
        }
        private void LoadDateTimePicker()
        {
            // thiết lập giá trị mặc định cho ngày đầu tiên trong tháng hiện tại
            dtpTu.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // thiết lập giá trị mặc định cho ngày cuối cùng trong tháng hiện tại
            dtpDen.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            // xem hóa đơn theo ngày đã chọn của datetimepicker
            string sql;
            sql = "SELECT a.MaHDBan, c.TenHang, a.SoLuong, a.DonGia, a.GiamGia, b.NgayBan, b.TongTien " +
                  "FROM tblChiTietHDBan AS a, tblHDBan AS b, tblHang AS c " +
                  "WHERE a.MaHDBan = b.MaHDBan AND a.MaHang = c.MaHang " +
                  "AND b.NgayBan >= '" + dtpTu.Value.ToString("yyyy-MM-dd") + "' " +
                  "AND b.NgayBan <= '" + dtpDen.Value.ToString("yyyy-MM-dd") + "' " +
                  "ORDER BY a.MaHDBan"; //Sắp xếp theo mã hóa đơn bán
            // Nạp dữ liệu vào DataTable tblDoanhThu
            tblDoanhThu = Functions.GetDataToTable(sql);
            if (tblDoanhThu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dgvDoanhThu.DataSource = tblDoanhThu; //Gán DataTable tblDoanhThu vào DataGridView
            dgvDoanhThu.Columns[0].HeaderText = "Mã hóa đơn bán";
            dgvDoanhThu.Columns[1].HeaderText = "Tên hàng";
            dgvDoanhThu.Columns[2].HeaderText = "Số lượng";
            dgvDoanhThu.Columns[3].HeaderText = "Đơn giá";
            dgvDoanhThu.Columns[4].HeaderText = "Giảm giá %";
            dgvDoanhThu.Columns[5].HeaderText = "Ngày bán";
            dgvDoanhThu.Columns[6].HeaderText = "Tổng tiền";
            dgvDoanhThu.Columns[0].Width = 130;
            dgvDoanhThu.Columns[1].Width = 130;
            dgvDoanhThu.Columns[2].Width = 80;
            dgvDoanhThu.Columns[3].Width = 90;
            dgvDoanhThu.Columns[4].Width = 90;
            dgvDoanhThu.Columns[5].Width = 90;
            dgvDoanhThu.Columns[6].Width = 90;
            dgvDoanhThu.AllowUserToAddRows = false; //Không cho phép thêm dòng mới
            // Thiết lập chế độ chỉnh sửa DataGridView
            dgvDoanhThu.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Cập nhật biểu đồ doanh thu
            LoadChartDoanhThu(); //Gọi hàm LoadChartDoanhThu để nạp dữ liệu vào biểu đồ doanh thu
        }
    }
}
