﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;

namespace QuanLyPhongKham
{
    public partial class Admin : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        connection connection = new connection();
        function function = new function();
        SqlDataAdapter da;
        SqlCommand cmd;
        OpenFileDialog open;
        int ID_Loaithuoc { get; set; }
        string hinhanh = null;
        DialogResult result;

        public Admin()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill a ExcelDataSource
            excelDataSource1.Fill();
        }

        public void Admin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'phongKhamDataSet.VatDung' table. You can move, or remove it, as needed.
            this.vatDungTableAdapter.Fill(this.phongKhamDataSet.VatDung);
            // TODO: This line of code loads data into the 'phongKhamDataSet.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Fill(this.phongKhamDataSet.NhanVien);
            load_qlyNhanVien_comB_Gioitinh();
            load_qlyNhanVien_comB_ViTri();
            load_qlyNhanVien_comB_QuyenTruyCap();
            // TODO: This line of code loads data into the 'phongKhamDataSet.Thuoc' table. You can move, or remove it, as needed.
            this.thuocTableAdapter.Fill(this.phongKhamDataSet.Thuoc);
            load_qlyThuoc_comB_loaithuoc();
            load_qlyThuoc_comB_donvitinh();
        }
        #region Ribbon Admin
        public void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)//nút refresh toàn form
        {
            refresh_qlyThuoc();
            refresh_qlyNhanVien();
            refresh_qlyVatDung();
        }
        private void barButtonItem1_BacSi_ItemClick(object sender, ItemClickEventArgs e)
        {           
            BacSi bacSi = new BacSi();
            
            bacSi.Show();
            this.Hide();
        }

        private void barButtonItem2_DuocSi_ItemClick(object sender, ItemClickEventArgs e)
        {           
            DuocSi duocSi = new DuocSi();
            duocSi.ShowDialog();
        }

        private void barButtonItem3_TiepTan_ItemClick(object sender, ItemClickEventArgs e)
        {            
            NhanVien nhanVien = new NhanVien();
            nhanVien.ShowDialog();
        }

        private void barButtonItem4_ThuNgan_ItemClick(object sender, ItemClickEventArgs e)
        {            
            NhanVienThuNgan nhanVienThuNgan = new NhanVienThuNgan();
            nhanVienThuNgan.ShowDialog();
        }
        private void barButtonItem2_NhapFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (admin_tabP_qlyThuoc.Focus() == true)
            //{
            //    function.Notice("Bạn muốn Nhập file Thuốc??", 1);
            //    open = new OpenFileDialog();
            //    open.InitialDirectory = "D:";
            //    open.Filter = "Select Images |*.xlsx";
            //    open.Multiselect = false;
            //    result = open.ShowDialog();
            //    open.RestoreDirectory = true;
            //    string link = open.FileName;
            //    function.OpenFile(link);
            //}
            //if (admin_tabP_qlyNhanvien.Focus() == true)
            //{
            //    function.Notice("Bạn muốn Nhập file Nhân Viên??", 1);
            //}
            //if (admin_tabP_qlyVatdung.Focus() == true)
            //{
            //    function.Notice("Bạn muốn Nhập file Vật Dụng??", 1);
            //}
        }

        private void barButtonItem1_XuatFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (admin_tabP_qlyThuoc.Focus() == true)
            {
                function.ToExcel("Bạn muốn xuất file Thuốc??", result, gridControl1);
            }
            if (admin_tabP_qlyNhanvien.Focus() == true)
            {
                function.ToExcel("Bạn muốn xuất file Nhân Viên??", result, gridControl2);
            }
            if (admin_tabP_qlyVatdung.Focus() == true)
            {
                function.ToExcel("Bạn muốn xuất file Vật Dụng??", result, gridControl3);
            }
        }
        #endregion

        #region form Thuốc
        private void gridView1_thuoc_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            function.CustomDrawRowIndicator(sender, e);
        }//vẽ cột số thứ tự cho gridview
        private void gridView1_thuoc_RowCountChanged(object sender, EventArgs e)
        {
            function.RowCountChanged(sender, e);
        }

        public void refresh_qlyThuoc()//hàm refresh tab QlyThuoc
        {
            function.ClearControl(panelControl1);
            load_qlyThuoc_comB_loaithuoc();
            load_qlyThuoc_comB_donvitinh();
            this.thuocTableAdapter.Fill(this.phongKhamDataSet.Thuoc);
            hinhanh = null;
            result = new DialogResult();
        }
        
        public void load_qlyThuoc_comB_loaithuoc()//load ComboBox loại thuốc
        {
            connection.connect();
            string query = @"select tenloaithuoc from loaithuoc";
            
            cmd = new SqlCommand(query, connection.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string tenloaithuoc = dr["tenloaithuoc"].ToString();
                qlyThuoc_comB_loaithuoc.Items.Add(tenloaithuoc) ;
            }
            dr.Close();
            connection.disconnect();
        }
        private void load_qlyThuoc_comB_donvitinh()//load ComboBox Đơn Vị Tính
        {
            qlyThuoc_comB_donvitinh.Items.Add("Viên");
            qlyThuoc_comB_donvitinh.Items.Add("Vĩ");
            qlyThuoc_comB_donvitinh.Items.Add("Hộp");
            qlyThuoc_comB_donvitinh.Items.Add("Thùng");
        }

        private void pictureBox1_Thuoc_DoubleClick(object sender, EventArgs e)//sự kiện khi nhấp double vào Picture Box
        {
            open = new OpenFileDialog();
            open.InitialDirectory = "D:";
            open.Filter = "Select Images |*.jpg||*.png";
            open.Multiselect = false;
            result = open.ShowDialog();
            open.RestoreDirectory = true;
            if (result == DialogResult.OK)
            {
                pictureBox1_Thuoc.Image = Image.FromStream(open.OpenFile());
                hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                pictureBox1_Thuoc.Image = new Bitmap(open.FileName);
            }
        }

        private void qlyThuoc_txt_SoLuong_KeyPress(object sender, KeyPressEventArgs e)//không được nhập kí tự
        {
            function.KoNhapKiTu(sender, e);
        }
        private void qlyThuoc_txt_DonGia_KeyPress(object sender, KeyPressEventArgs e)//không được nhập kí tự
        {
            function.KoNhapKiTu(sender, e);
        }

        private void qlyThuoc_comB_loaithuoc_SelectedIndexChanged(object sender, EventArgs e)//Lấy mã loại thuốc khi chọn ComboBox Loại Thuốc
        {
            var tenloaithuoc = qlyThuoc_comB_loaithuoc.SelectedItem;
            string query = @"select masoloaithuoc from loaithuoc where tenloaithuoc = N'" + tenloaithuoc + "'";
            connection.connect();
            DataTable dt = connection.SQL(query);
            ID_Loaithuoc = int.Parse(dt.Rows[0][0].ToString());
        }

        private void gridView1_thuoc_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)//Sự kiện khi chọn vào hàng trong gridview
        {

            string ID = gridView1_thuoc.GetFocusedRowCellValue("MaSoThuoc").ToString();
            qlyThuoc_txt_tenthuoc.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["TenThuoc"]).ToString();
            qlyThuoc_txt_SoLuong.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["SoLuong"]).ToString();
            qlyThuoc_txt_DonGia.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["DonGia"]).ToString();
            qlyThuoc_txt_cachdung.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["CachDung"]).ToString();
            qlyThuoc_dtP_ngaytao.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["NgayNhap"]).ToString();
            var MaSoLoaiThuoc = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["MaSoLoaiThuoc"]).ToString();
            qlyThuoc_comB_donvitinh.Text = gridView1_thuoc.GetRowCellValue(gridView1_thuoc.FocusedRowHandle, gridView1_thuoc.Columns["DonViTinh"]).ToString();


            connection.connect();
            string laytenloaithuoc = @"select tenloaithuoc  from loaithuoc where masoloaithuoc = " + MaSoLoaiThuoc;
            if(MaSoLoaiThuoc!="")
            {
                DataTable dt = connection.SQL(laytenloaithuoc);
                qlyThuoc_comB_loaithuoc.Text = dt.Rows[0][0].ToString();
            }
            else {
                qlyThuoc_comB_loaithuoc.Text="";
            }

            string layhinhanh = @"select hinhanh from thuoc where MaSoThuoc = " + ID;
            cmd = new SqlCommand(layhinhanh, connection.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if(dr["hinhanh"].ToString()!="")//kiểm tra đường dẫn hình ảnh từ SQL
                {
                    if(File.Exists(Application.StartupPath + @"\Hinh\Thuoc\" + dr["hinhanh"].ToString()))//kiểm tra hình ảnh có trong thư mục hay không
                    {
                        //có thì sẽ load hình ảnh vào pictureBox
                        pictureBox1_Thuoc.Image = new Bitmap(Application.StartupPath + @"\Hinh\Thuoc\" + dr["hinhanh"].ToString());
                    }
                    else
                    {
                        //không thì hiện thông báo
                        function.Notice("Không có file " + dr["hinhanh"].ToString() + " trong thư mục", 1);
                    }
                }
                else
                {
                    //chưa insert hình ảnh thì picturebox sẽ không hiện gì hết
                    pictureBox1_Thuoc.Image = null;
                    continue;
                }
            }
            dr.Close();
           
            connection.disconnect();
        }
        
        private void qlyThuoc_btn_themloaithuoc_Click(object sender, EventArgs e)//chuyển sang form thêm loại thuốc
        {
            ThemLoaiThuoc themLoaiThuoc = new ThemLoaiThuoc();
            themLoaiThuoc.Show();
        }

        private void qlyThuoc_btn_taomoi_Click(object sender, EventArgs e)//sự kiện nút Tạo mới
        {
            
            if (function.checkNull(panelControl1) == true)
            {
                connection.connect();
                //kiểm tra tên thuốc có bị trùng hay không
                string checktenthuoc = @"select top 1 tenthuoc from thuoc where tenthuoc = N'" + qlyThuoc_txt_tenthuoc.Text + "'";
                cmd = new SqlCommand(checktenthuoc, connection.con);
                SqlDataReader dr = cmd.ExecuteReader();
                
                //hinhanh = null;
                if (pictureBox1_Thuoc.Image != null)//kiểm tra picturebox có rỗng hay không
                {
                    if (result == DialogResult.OK)
                    {
                        hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                        string previewPath = Application.StartupPath + @"\Hinh\Thuoc\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else { }                    
                }
                else { }

                if (dr.Read())
                {
                    function.Notice("Bạn nhập trùng tên thuốc!", 0);
                }
                else
                {
                    dr.Close();
                    string query = @"insert into thuoc(masoloaithuoc,tenthuoc,soluong,dongia,donvitinh,ngaynhap,cachdung,hinhanh) values ("
                        + ID_Loaithuoc + ",N'"
                        + qlyThuoc_txt_tenthuoc.Text + "',"
                        + qlyThuoc_txt_SoLuong.Text + ","
                        + qlyThuoc_txt_DonGia.Text + ",N'"
                        + qlyThuoc_comB_donvitinh.Text + "','"
                        + qlyThuoc_dtP_ngaytao.Text + "',N'"
                        + qlyThuoc_txt_cachdung.Text + "',N'"
                        + hinhanh+"')";
                    connection.insert(query);
                    connection.disconnect();
                    refresh_qlyThuoc();
                }
            }
            
        }

        private void qlyThuoc_btn_xoa_Click(object sender, EventArgs e)//sự kiện nút xóa
        {
            try
            {
                string ID = gridView1_thuoc.GetFocusedRowCellValue("MaSoThuoc").ToString();

                connection.connect();
                string query = @"delete from thuoc where masothuoc = " + ID;
                connection.delete(query);
                connection.disconnect();
                refresh_qlyThuoc();
            }
            catch (Exception ex)
            { throw ex; }
        }
      
        private void qlyThuoc_btn_capnhat_Click(object sender, EventArgs e)//sự kiện nút cập nhật
        {
            if (function.checkNull(panelControl1) == true)//kiểm tra các thành phần có rỗng hay không
            {
                connection.connect();
                int ID = int.Parse(gridView1_thuoc.GetFocusedRowCellValue("MaSoThuoc").ToString());

                var tenloaithuoc = qlyThuoc_comB_loaithuoc.SelectedItem;
                string laymasoloaithuoc = @"select masoloaithuoc from loaithuoc where tenloaithuoc = N'" + tenloaithuoc + "'";
                DataTable dt = connection.SQL(laymasoloaithuoc);
                ID_Loaithuoc = int.Parse(dt.Rows[0][0].ToString());

                
                if (pictureBox1_Thuoc.Image != null)//kiểm tra picturebox có rỗng hay không
                {                    
                    if (result == DialogResult.OK)
                    {
                        string previewPath = Application.StartupPath + @"\Hinh\Thuoc\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else
                    {
                        string layhinhanh = @"select hinhanh from thuoc where MaSoThuoc = " + ID;
                        DataTable dt1 = connection.SQL(layhinhanh);
                        hinhanh = dt1.Rows[0][0].ToString();
                    }
                }
                else { }

                string query = @"update Thuoc set TenThuoc = N'" + qlyThuoc_txt_tenthuoc.Text +
                "', CachDung = N'" + qlyThuoc_txt_cachdung.Text + "'," +
                "SoLuong =" + qlyThuoc_txt_SoLuong.Text + "," +
                "DonGia =" + qlyThuoc_txt_DonGia.Text + "," +
                "NgayNhap ='" + qlyThuoc_dtP_ngaytao.Text + "'," +
                "MaSoLoaiThuoc =" + ID_Loaithuoc + "," +
                "DonViTinh=N'" + qlyThuoc_comB_donvitinh.Text + "'," +
                "HinhAnh = N'"+hinhanh+"'"+
                " where MaSoThuoc =" + ID;
                connection.sql(query);
                connection.disconnect();
                refresh_qlyThuoc();
            }                  
        }
        #endregion

        #region form Nhân Viên
        private void qlyNhanvien_txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            function.KoNhapKiTu(sender, e);
        }
        private void load_qlyNhanVien_comB_Gioitinh()//load ComboBox giới Tính
        {
            qlyNhanvien_comB_gioitinh.Items.Add("Nam");
            qlyNhanvien_comB_gioitinh.Items.Add("Nữ");
            qlyNhanvien_comB_gioitinh.Items.Add("Khác");
            
        }
        private void load_qlyNhanVien_comB_ViTri()//load ComboBox Vị trí 
        {
            qlyNhanvien_comB_vitri.Items.Add("Điều dưỡng");
            qlyNhanvien_comB_vitri.Items.Add("Bác Sĩ");
            qlyNhanvien_comB_vitri.Items.Add("Dược Sĩ");
            qlyNhanvien_comB_vitri.Items.Add("Thu Ngân");
        }
        private void load_qlyNhanVien_comB_QuyenTruyCap()//load ComboBox Quyền truy cập
        {
            qlyNhanvien_comB_QuyenTruyCap.Items.Add("Điều dưỡng   (2)");
            qlyNhanvien_comB_QuyenTruyCap.Items.Add("Bác Sĩ       (3)");
            qlyNhanvien_comB_QuyenTruyCap.Items.Add("Dược Sĩ    (4)");
            qlyNhanvien_comB_QuyenTruyCap.Items.Add("Thu Ngân (5)");
        }
        private void refresh_qlyNhanVien()
        {
            function.ClearControl(panelControl3_NhanVien);
            this.nhanVienTableAdapter.Fill(this.phongKhamDataSet.NhanVien);
            load_qlyNhanVien_comB_Gioitinh();
            load_qlyNhanVien_comB_ViTri();
            load_qlyNhanVien_comB_QuyenTruyCap();
            hinhanh = null;
            result = new DialogResult();
        }
        private void gridView1_NhanVien_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            function.CustomDrawRowIndicator(sender, e);
        }

        private void gridView1_NhanVien_RowCountChanged(object sender, EventArgs e)
        {
            function.RowCountChanged(sender, e);
        }
        int quyentruycap;
        private void qlyNhanvien_comB_QuyenTruyCap_SelectedIndexChanged(object sender, EventArgs e)//lấy quyền truy cập
        {
            
            if(qlyNhanvien_comB_QuyenTruyCap.SelectedIndex == 0)
            {
                quyentruycap = 2;//Tiếp tân
            }
            else if (qlyNhanvien_comB_QuyenTruyCap.SelectedIndex == 1)
            {
                quyentruycap = 3;//Bác sĩ
            }
            else if (qlyNhanvien_comB_QuyenTruyCap.SelectedIndex == 2)
            {
                quyentruycap = 4;//Dược sĩ
            }
            else if (qlyNhanvien_comB_QuyenTruyCap.SelectedIndex == 3)
            {
                quyentruycap = 5;//Thu ngân
            }
        }
        private void pictureBox1_NhanVien_DoubleClick(object sender, EventArgs e)
        {
            open = new OpenFileDialog();
            open.InitialDirectory = "D:";
            open.Filter = "Select Images |*.jpg||*.png";
            open.Multiselect = false;
            result = open.ShowDialog();
            open.RestoreDirectory = true;
            if (result == DialogResult.OK)
            {
                pictureBox1_NhanVien.Image = Image.FromStream(open.OpenFile());

                hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                pictureBox1_NhanVien.Image = new Bitmap(open.FileName);
            }
        }
        private void qlyNhanvien_btn_taomoi_Click(object sender, EventArgs e)
        {
            if(function.checkNull(panelControl3_NhanVien)==true)
            {
                connection.connect();

                var matkhau = function.toMD5(qlyNhanvien_txt_matkhau.Text);

                //result = new DialogResult();
                if (pictureBox1_NhanVien.Image != null)//kiểm tra picturebox có rỗng hay không
                {
                    if (result == DialogResult.OK)
                    {
                        hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                        string previewPath = Application.StartupPath + @"\Hinh\NhanVien\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else { }
                }
                else { }

                string query = @"insert into NhanVien(TenNhanVien, NgaySinh, ViTri, DiaChi, SoDienThoai, QuyenTruyCap, TaiKhoan, MatKhau, NgayTao, GioiTinh,HinhAnh) values"
                    +"(N'"+ qlyNhanvien_txt_hoten.Text  +"',"
                    + "'"+  qlyNhanvien_dtP_ngaysinh.Text   +"',"
                    + "N'"+ qlyNhanvien_comB_vitri.Text +"',"
                    + "N'"+ qlyNhanvien_txt_diachi.Text +"',"
                    + qlyNhanvien_txt_sdt.Text +","
                    + quyentruycap + ","
                    + "N'"+ qlyNhanvien_txt_taikhoan.Text + "',"
                    + "'"+ matkhau + "',"
                    + "'"+ qlyNhanvien_dtP_ngaytao.Text + "',"
                    + "N'"+ qlyNhanvien_comB_gioitinh.Text+"',"
                    + "N'"+ hinhanh + "')";
                connection.insert(query);
                connection.disconnect();
                refresh_qlyNhanVien();
            }
        }

        private void gridView1_NhanVien_RowClick(object sender, RowClickEventArgs e)
        {
            string ID = gridView1_NhanVien.GetFocusedRowCellValue("MaSoNhanVien").ToString();
            qlyNhanvien_txt_hoten.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["TenNhanVien"]).ToString();
            qlyNhanvien_comB_gioitinh.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["GioiTinh"]).ToString();
            qlyNhanvien_txt_sdt.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["SoDienThoai"]).ToString();
            qlyNhanvien_comB_vitri.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["ViTri"]).ToString();
            qlyNhanvien_dtP_ngaysinh.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["NgaySinh"]).ToString();
            qlyNhanvien_txt_diachi.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["DiaChi"]).ToString();
            qlyNhanvien_dtP_ngaytao.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["NgayTao"]).ToString();
            qlyNhanvien_txt_taikhoan.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["TaiKhoan"]).ToString();
            qlyNhanvien_txt_matkhau.Text =function.toMD5( gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["MatKhau"]).ToString());
            qlyNhanvien_comB_QuyenTruyCap.Text = gridView1_NhanVien.GetRowCellValue(gridView1_NhanVien.FocusedRowHandle, gridView1_NhanVien.Columns["QuyenTruyCap"]).ToString();

            connection.connect();

            string layhinhanh = @"select hinhanh from NhanVien where MaSoNhanVien = " + ID;
            cmd = new SqlCommand(layhinhanh, connection.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["hinhanh"].ToString() != "")//kiểm tra đường dẫn hình ảnh từ SQL
                {
                    if (File.Exists(Application.StartupPath + @"\Hinh\NhanVien\" + dr["hinhanh"].ToString()))//kiểm tra hình ảnh có trong thư mục hay không
                    {
                        //có thì sẽ load hình ảnh vào pictureBox
                        pictureBox1_NhanVien.Image = new Bitmap(Application.StartupPath + @"\Hinh\NhanVien\" + dr["hinhanh"].ToString());
                    }
                    else
                    {
                        //không thì hiện thông báo
                        function.Notice("Không có file " + dr["hinhanh"].ToString() + " trong thư mục", 1);
                    }
                }
                else
                {
                    //chưa insert hình ảnh thì picturebox sẽ không hiện gì hết
                    pictureBox1_NhanVien.Image = null;
                    continue;
                }
            }
            dr.Close();

            connection.disconnect();
        }

        private void qlyNhanvien_btn_capnhat_Click(object sender, EventArgs e)
        {
            if (function.checkNull(panelControl3_NhanVien) == true)//kiểm tra các thành phần có rỗng hay không
            {
                connection.connect();
                string ID = gridView1_NhanVien.GetFocusedRowCellValue("MaSoNhanVien").ToString();

                var matkhau = function.toMD5(qlyNhanvien_txt_matkhau.Text);


                if (pictureBox1_NhanVien.Image != null)//kiểm tra picturebox có rỗng hay không
                {
                    if (result == DialogResult.OK)
                    {
                        string previewPath = Application.StartupPath + @"\Hinh\NhanVien\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else
                    {
                        string layhinhanh = @"select hinhanh from NhanVien where MaSoNhanVien = " + ID;
                        DataTable dt1 = connection.SQL(layhinhanh);
                        hinhanh = dt1.Rows[0][0].ToString();
                    }
                }
                else { }

                string query = @"update NhanVien set TenNhanVien = N'" + qlyNhanvien_txt_hoten.Text + "',"+
                "GioiTinh = N'" + qlyNhanvien_comB_gioitinh.Text + "'," +
                "SoDienThoai =" + qlyNhanvien_txt_sdt.Text + "," +
                "ViTri = N'" + qlyNhanvien_comB_vitri.Text + "'," +
                "NgaySinh ='" + qlyNhanvien_dtP_ngaysinh.Text + "'," +
                "DiaChi = N'" + qlyNhanvien_txt_diachi.Text + "'," +
                "NgayTao='" + qlyNhanvien_dtP_ngaytao.Text + "'," +
                "TaiKhoan = N'" + qlyNhanvien_txt_taikhoan.Text + "'," +
                "MatKhau = N'" + matkhau + "'," +
                "QuyenTruyCap =" + quyentruycap + ","+
                "HinhAnh = N'" + hinhanh + "'"+
                " where MaSoNhanVien =" + ID;
                connection.sql(query);
                connection.disconnect();
                refresh_qlyNhanVien();

            }
        }

        private void qlyNhanvien_btn_xoa_Click(object sender, EventArgs e)
        {

            string ID = gridView1_NhanVien.GetFocusedRowCellValue("MaSoNhanVien").ToString();

            connection.connect();
            string query = @"delete from NhanVien where MaSoNhanVien = " + ID;
            connection.delete(query);
            connection.disconnect();
            refresh_qlyNhanVien();

        }
        #endregion

        #region form Vật dụng
        private void refresh_qlyVatDung()
        {
            function.ClearControl(panelControl5_VatDung);
            this.vatDungTableAdapter.Fill(this.phongKhamDataSet.VatDung);
            hinhanh = null;
            result = new DialogResult();
        }

        private void qlyVatdung_txt_sonamsudung_KeyPress(object sender, KeyPressEventArgs e)
        {
            function.KoNhapKiTu(sender, e);
        }

        private void qlyVatdung_txt_soluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            function.KoNhapKiTu(sender, e);
        }

        private void gridView1_VatDung_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            function.CustomDrawRowIndicator(sender, e);
        }

        private void gridView1_VatDung_RowCountChanged(object sender, EventArgs e)
        {
            function.RowCountChanged(sender, e);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            open = new OpenFileDialog();
            open.InitialDirectory = "D:";
            open.Filter = "Select Images |*.jpg||*.png";
            open.Multiselect = false;
            result = open.ShowDialog();
            open.RestoreDirectory = true;
            if (result == DialogResult.OK)
            {
                pictureBox1_VatDung.Image = Image.FromStream(open.OpenFile());

                hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                pictureBox1_VatDung.Image = new Bitmap(open.FileName);
            }
        }

        private void gridView1_VatDung_RowClick(object sender, RowClickEventArgs e)
        {
            string ID = gridView1_VatDung.GetFocusedRowCellValue("MaSoVatDung").ToString();
            qlyVatdung_txt_tenvatdung.Text = gridView1_VatDung.GetRowCellValue(gridView1_VatDung.FocusedRowHandle, gridView1_VatDung.Columns["TenVatDung"]).ToString();
            qlyVatdung_txt_soluong.Text = gridView1_VatDung.GetRowCellValue(gridView1_VatDung.FocusedRowHandle, gridView1_VatDung.Columns["SoLuong"]).ToString();
            qlyVatdung_txt_sonamsudung.Text = gridView1_VatDung.GetRowCellValue(gridView1_VatDung.FocusedRowHandle, gridView1_VatDung.Columns["SoNamSuDung"]).ToString();
            qlyVatdung_dtP_ngaytao.Text = gridView1_VatDung.GetRowCellValue(gridView1_VatDung.FocusedRowHandle, gridView1_VatDung.Columns["NgayTao"]).ToString();

            connection.connect();

            string layhinhanh = @"select hinhanh from VatDung where MaSoVatDung = " + ID;
            cmd = new SqlCommand(layhinhanh, connection.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["hinhanh"].ToString() != "")//kiểm tra đường dẫn hình ảnh từ SQL
                {
                    if (File.Exists(Application.StartupPath + @"\Hinh\VatDung\" + dr["hinhanh"].ToString()))//kiểm tra hình ảnh có trong thư mục hay không
                    {
                        //có thì sẽ load hình ảnh vào pictureBox
                        pictureBox1_VatDung.Image = new Bitmap(Application.StartupPath + @"\Hinh\VatDung\" + dr["hinhanh"].ToString());
                    }
                    else
                    {
                        //không thì hiện thông báo
                        function.Notice("Không có file " + dr["hinhanh"].ToString() + " trong thư mục", 1);
                    }
                }
                else
                {
                    //chưa insert hình ảnh thì picturebox sẽ không hiện gì hết
                    pictureBox1_VatDung.Image = null;
                    continue;
                }
            }
            dr.Close();

            connection.disconnect();
        }

        private void qlyVatdung_btn_taomoi_Click(object sender, EventArgs e)
        {
            if (function.checkNull(panelControl5_VatDung) == true)
            {
                connection.connect();

                if (pictureBox1_VatDung.Image != null)//kiểm tra picturebox có rỗng hay không
                {
                    if (result == DialogResult.OK)
                    {
                        hinhanh = open.FileName.Substring(open.FileName.LastIndexOf("\\") + 1, open.FileName.Length - open.FileName.LastIndexOf("\\") - 1);
                        string previewPath = Application.StartupPath + @"\Hinh\VatDung\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else { }
                }
                else { }

                string query = @"insert into VatDung(TenVatDung, SoLuong, SoNamSuDung, NgayTao,HinhAnh) values"
                    + "(N'" + qlyVatdung_txt_tenvatdung.Text + "',"
                    + qlyVatdung_txt_soluong.Text + ","
                    + qlyVatdung_txt_sonamsudung.Text + ","
                    + "'" + qlyVatdung_dtP_ngaytao.Text + "',"
                    + "N'" + hinhanh + "')";
                connection.insert(query);
                connection.disconnect();
                refresh_qlyVatDung();
            }
        }

        private void qlyVatdung_btn_capnhat_Click(object sender, EventArgs e)
        {
            if (function.checkNull(panelControl5_VatDung) == true)//kiểm tra các thành phần có rỗng hay không
            {
                connection.connect();
                string ID = gridView1_VatDung.GetFocusedRowCellValue("MaSoVatDung").ToString();


                if (pictureBox1_VatDung.Image != null)//kiểm tra picturebox có rỗng hay không
                {
                    if (result == DialogResult.OK)
                    {
                        string previewPath = Application.StartupPath + @"\Hinh\VatDung\" + hinhanh;
                        string linkHinhAnh = open.FileName;
                        File.Copy(linkHinhAnh, previewPath, true);//copy file ảnh vào thư mục project
                    }
                    else
                    {
                        string layhinhanh = @"select hinhanh from VatDung where MaSoVatDung = " + ID;
                        DataTable dt1 = connection.SQL(layhinhanh);
                        hinhanh = dt1.Rows[0][0].ToString();
                    }
                }
                else { }

                string query = @"update VatDung set TenVatDung = N'"+qlyVatdung_txt_tenvatdung.Text+"',"+
                    "SoLuong =" + qlyVatdung_txt_soluong.Text + ","+
                    "SoNamSuDung =" + qlyVatdung_txt_sonamsudung.Text + ","+
                    "NgayTao =" + "'" + qlyVatdung_dtP_ngaytao.Text + "',"+
                    "HinhAnh= N'" + hinhanh + "'"+                  
                    " where MaSoVatDung =" + ID;
                connection.sql(query);
                connection.disconnect();
                refresh_qlyVatDung();

            }
        }

        private void qlyVatdung_btn_xoa_Click(object sender, EventArgs e)
        {
            string ID = gridView1_VatDung.GetFocusedRowCellValue("MaSoVatDung").ToString();

            connection.connect();
            string query = @"delete from VatDung where MaSoVatDung = " + ID;
            connection.delete(query);
            connection.disconnect();
            refresh_qlyVatDung();
        }

        #endregion



    }
}