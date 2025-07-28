using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //Sử dụng đối tượng MessageBox
using System.Data;
using System.Data.SqlClient;

namespace QuanLyBanHang.Class
{
    class Functions
    {
        public static SqlConnection conn; //Khai báo biến kết nối

        //Hàm kết nối
        public static void Connect()
        {
            conn = new SqlConnection();
            conn.ConnectionString = Properties.Settings.Default.QuanLyCuaHangBanMayTinh;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open(); //Mở kết nối
                MessageBox.Show("Kết nối thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kết nối thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Hàm ngắt kết nối
        public static void Disconnect()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close(); //Đóng kết nối
                conn.Dispose(); //Giải phóng tài nguyên
                conn = null; //Gán giá trị null cho biến kết nối
                MessageBox.Show("Ngắt kết nối thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ngắt kết nối thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Hàm thực hiện câu lệnh SQL
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn); //Tạo đối tượng SqlDataAdapter
            DataTable dt = new DataTable(); //Tạo đối tượng DataTable
            da.Fill(dt); //Đổ dữ liệu vào DataTable
            return dt; //Trả về DataTable
        }

        //Insert, Update, Delete
        public static void RunSQL(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn); //Tạo đối tượng SqlCommand
            cmd.Connection = conn; //Gán kết nối cho SqlCommand
            cmd.CommandText = sql; //Gán câu lệnh SQL cho SqlCommand
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Dispose(); //Giải phóng tài nguyên
                cmd = null; //Gán giá trị null cho SqlCommand
            }
        }

        //Hàm kiểm tra dữ liệu có tồn tại hay không
        public static bool CheckKey(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn); //Tạo đối tượng SqlCommand
            cmd.Connection = conn; //Gán kết nối cho SqlCommand
            cmd.CommandText = sql; //Gán câu lệnh SQL cho SqlCommand
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader(); //Thực hiện câu lệnh SQL
                if (reader.HasRows) //Nếu có dữ liệu
                {
                    reader.Close(); //Đóng SqlDataReader
                    return true; //Trả về true
                }
                else
                {
                    reader.Close(); //Đóng SqlDataReader
                    return false; //Trả về false
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; //Trả về false
            }
        }

        public static void FillComboBox(ComboBox cbo, string sql, string ma, string ten)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn); //Tạo đối tượng SqlDataAdapter
            DataTable table = new DataTable(); //Tạo đối tượng DataTable
            da.Fill(table); //Đổ dữ liệu vào DataTable
            cbo.DataSource = table; //Gán DataTable cho ComboBox
            cbo.DisplayMember = ten; //Gán tên hiển thị cho ComboBox
            cbo.ValueMember = ma; //Gán mã cho ComboBox
        }

        public static string GetFieldValues(string sql)
        {
            string result = "";
            SqlCommand cmd = new SqlCommand(sql, conn); // Create SqlCommand object
            SqlDataReader reader;

            try
            {
                reader = cmd.ExecuteReader(); // Execute the SQL command
                if (reader.Read()) // Read the data
                {
                    // Check the type of the first column and convert it to a string
                    if (!reader.IsDBNull(0))
                    {
                        object value = reader.GetValue(0);
                        result = value.ToString(); // Safely convert any type to string
                    }
                }
                reader.Close(); // Close the reader
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Dispose(); // Release resources
            }

            return result; // Return the result
        }

        //Hàm chuyển đổi từ số thành chữ
        //123 => một trăm hai ba đồng
        //1,123,000=>một triệu một trăm hai ba nghìn đồng
        //1,123,345,000 => một tỉ một trăm hai ba triệu ba trăm bốn lăm ngàn đồng
        public static string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');

        //Viết hàm chuyển số hàng chục, giá trị truyền vào là số cần chuyển và một biến đọc phần lẻ hay không ví dụ 101 => một trăm lẻ một
        private static string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + mNumText[chuc] + " mươi";
                if (donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + mNumText[donvi];
            }
            return chuoi;
        }
        private static string DocHangTram(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + mNumText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }
        private static string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
            //Lấy phần dư sau số hàng nghin 
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }
        public static string ChuyenSoSangChuoi(double so)
        {
            if (so == 0)
                return mNumText[0];
            string chuoi = "", hauto = "";
            Int64 ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (ty > 0);
            return chuoi + " đồng";
        }

        //Hàm tạo khóa có dạng: TientoNgaythangnam_giophutgiay
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }

        //Chuyển đổi từ PM sang dạng 24h
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

    }
}
