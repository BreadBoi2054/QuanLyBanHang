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

namespace QuanLyBanHang
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Functions.Connect(); //Kết nối CSDL

        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Functions.Disconnect(); //Ngắt kết nối CSDL
            Application.Exit(); //Đóng ứng dụng
        }


        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            frmDMHangHoa f = new frmDMHangHoa();
            f.Show(); //Hiện form danh mục hàng hóa
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmDMNhanVien f = new frmDMNhanVien();
            f.Show(); //Hiện form danh mục nhân viên
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmDMKhachHang f = new frmDMKhachHang();
            f.Show(); //Hiện form danh mục khách hàng
        }

        private void mnuHoaDonBan_Click(object sender, EventArgs e)
        {
            frmDMHoaDonBan f = new frmDMHoaDonBan();
            //f.MdiParent = this; //Thiết lập form cha
            f.Show(); //Hiện form danh mục hóa đơn bán
        }

        private void tìmKiếmHoáĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTimKiemHoaDon f = new frmTimKiemHoaDon();
            f.Show(); //Hiện form tìm kiếm hóa đơn
        }

        private void doanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoanhThu f = new frmDoanhThu();
            f.Show(); //Hiện form tìm kiếm hóa đơn
        }
    }
}