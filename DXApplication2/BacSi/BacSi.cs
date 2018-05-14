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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
namespace QuanLyPhongKham
{
    public partial class BacSi : DevExpress.XtraBars.Ribbon.RibbonForm 
    {
        connection connection = new connection();
        SqlDataAdapter da;
        DataSet ds;
        BindingSource bindingSource = new BindingSource();
        SqlCommand cmd;
        DangNhap dangNhap = new DangNhap();
        DonThuoc donThuoc = new DonThuoc();
        function function = new function();
        int quyentruycap;
        public static bool RowClick = false;

        public static string Ho_BenhNhan { get; set; }
        public static string Ten_BenhNhan { get; set; }
        public static string XetNghiem_BenhNhan { get; set; }
        public static string ChuanDoan_BenhNhan { get; set; }
        public static string GhiChu_BenhNhan { get; set; }
        public static string NamSinh_BenhNhan { get; set; }
        public static string GioiTinh_BenhNhan { get; set; }
        public static string DiaChi_BenhNhan { get; set; }
        public static string BacSiKham_BenhNhan { get; set; }
        public static int ID_MSKB { get; set; }
        public static int ID_MSBN { get; set; }
        public static string TienThuoc { get; set; }
        public BacSi()
        {
            InitializeComponent();

            Load_HoSoKhamBenh();
            // This line of code is generated by Data Source Configuration Wizard
            hoSoKhamBenhTableAdapter1.Fill(phongKhamDataSet1.HoSoKhamBenh);
        }
        private void BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay_RowCountChanged(object sender, EventArgs e)
        {
            function.RowCountChanged(sender, e);

        }

        private void BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            function.CustomDrawRowIndicator(sender, e);
        }
        private void Load_HoSoKhamBenh()
        {
            connection.connect();
            string ngay = DateTime.Now.Day.ToString("d2");
            string thang = DateTime.Now.Month.ToString("d2");
            string nam = DateTime.Now.Year.ToString();
            //quyentruycap = DangNhap.quyentruycap;
            string Load_Data = @"SELECT     DISTINCT   HSKB.MaSoKhamBenh, HSKB.MaSoBenhNhan,NV.TenNhanVien, BN.Ho, BN.Ten, BN.GioiTinh," +
                                                    " BN.NamSinh, HSKB.NgayGioKham, HSKB.MaSoBacSi, HSKB.XetNghiem," +
                                                    " HSKB.ChuanDoan, HSKB.TienKham, HSKB.NgayTaiKham, HSKB.GhiChu, " +
                                                    "HSKB.KiemTraKham, HSKB.LiDoKham, BN.DiaChi, BN.SoDienThoai, BN.HinhAnh, HSKB.KiemTraTaiKham" +
                                " FROM            HoSoKhamBenh HSKB LEFT JOIN" +
                                                    " BenhNhan BN ON BN.MaSoBenhNhan = HSKB.MaSoBenhNhan" +
                                                    " left join NhanVien NV on HSKB.MaSoBacSi = NV.MaSoNhanVien" +
                                                    " where HSKB.NgayGioKham like '" + ngay + "/" + thang + "/" + nam + "%'";
            da = new SqlDataAdapter(Load_Data, connection.con);
            ds = new DataSet();
            ds.Clear();
            da.Fill(ds, "HoSoKhamBenh");

            bindingSource.DataSource = ds.Tables["HoSoKhamBenh"];
            BacSi_gridControl_danhsachBenhNhanDaKhamTrongNgay.DataSource = bindingSource;            
            connection.disconnect();

            GanGiaTri();
        }

        private void Refresh_BacSi()
        {
            function.ClearControl(panelControl2);
            Load_HoSoKhamBenh();
            RowClick = false;
        }

        private void GanGiaTri()
        {
            txtTienThuoc.Text = TienThuoc;
            BacSiKham_BenhNhan = DangNhap.TenBacSi;
        }
        private void load_TienThuoc()
        {
            string query = @"select DT.TongTienThuoc "+
                            " from DonThuoc DT Left Join HoSoKhamBenh HSKB on DT.MaSoKhamBenh = HSKB.MaSoKhamBenh"+
                            " where HSKB.MaSoKhamBenh ="+ ID_MSKB;
            //connection.connect();
            DataTable dt = connection.SQL(query);
            if (dt.Rows.Count > 0)//tồn tại
            {
                txtTienThuoc.Text = dt.Rows[0][0].ToString();
            }
            //connection.disconnect();
        }

        private void btn_TaoDonThuoc_Click(object sender, EventArgs e)
        {
            if(RowClick ==false)
            {
                function.Notice("Bạn nên chọn bệnh nhân trước!", 1);
            }
            
            else
            {
                XetNghiem_BenhNhan = txt_xetnghiem.Text;
                ChuanDoan_BenhNhan = txt_chuandoan.Text;
                GhiChu_BenhNhan = txt_GhiChu.Text;

                
                DonThuoc donThuoc = new DonThuoc();
                donThuoc.Show();
                RowClick = false;
            }
            
        }

        private void BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay_RowClick(object sender, RowClickEventArgs e)
        {
            RowClick = true;
            ID_MSBN = int.Parse(BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("MaSoBenhNhan").ToString());
            //txt_TenBacSi.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("TenNhanVien").ToString();
            txt_ho.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("Ho").ToString();
            txt_ten.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("Ten").ToString();
            //txt_xetnghiem.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("XetNghiem").ToString();
            //txt_chuandoan.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("ChuanDoan").ToString();
            //txt_GhiChu.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("GhiChu").ToString();
            dtP_NgayTaiKham.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("NgayTaiKham").ToString();
            txt_TienKham.Text = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("TienKham").ToString();

            ID_MSKB =int.Parse( BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("MaSoKhamBenh").ToString());
            Ho_BenhNhan = txt_ho.Text;
            Ten_BenhNhan = txt_ten.Text;            
            NamSinh_BenhNhan = BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("NamSinh").ToString();
            GioiTinh_BenhNhan= BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("GioiTinh").ToString();
            DiaChi_BenhNhan= BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("DiaChi").ToString();
            //BacSiKham_BenhNhan= BacSi_gridView_danhsachBenhNhanDaKhamTrongNgay.GetFocusedRowCellValue("TenNhanVien").ToString();
                       

            connection.connect();
            load_TienThuoc();

            string layhinhanh = @"select hinhanh from BenhNhan where MaSoBenhNhan = " + ID_MSBN;
            cmd = new SqlCommand(layhinhanh, connection.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                if(dr["hinhanh"].ToString() != "")
                {
                    if(File.Exists(Application.StartupPath + @"\Hinh\BenhNhan\" + dr["hinhanh"].ToString()))
                    {
                        pictureBox1_BenhNhan.Image = new Bitmap(Application.StartupPath + @"\Hinh\BenhNhan\" + dr["hinhanh"].ToString());
                    }
                    else
                    {
                        function.Notice("Không có file " + dr["hinhanh"].ToString() + " trong thư mục",1);
                    }
                }
                else
                {
                    pictureBox1_BenhNhan.Image = null;
                    continue;
                }
            }
            dr.Close();
            connection.disconnect();
        }

        private void btn_HoanTat_Click(object sender, EventArgs e)
        {
            if (function.checkNull(panelControl2) == true)
            {
                string query = @"update HoSoKhamBenh set " +
                                " XetNghiem = N'" + txt_xetnghiem.Text + "'," +
                                " ChuanDoan = N'" + txt_chuandoan.Text + "'," +
                                " GhiChu = N'" + txt_GhiChu.Text + "'," +
                                " NgayTaiKham = N'" + dtP_NgayTaiKham.Text + "'," +
                                " TienKham  = " + txt_TienKham.Text + "," +
                                " KiemTraKham = 1" + "," +
                                " MaSoBacSi = " + DangNhap.MaSoBacSi +
                                " where MaSoBenhNhan =" + ID_MSBN + " and " + " MaSoKhamBenh = " + ID_MSKB;
                connection.connect();
                connection.sql(query);
                connection.disconnect();
                Refresh_BacSi();
            }
        }       

       

        private void txt_TienKham_KeyPress(object sender, KeyPressEventArgs e)
        {
            function.KoNhapKiTu(sender, e);
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Refresh_BacSi();
        }

        private void bbtn_TaoDonThuoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            btn_TaoDonThuoc_Click(sender, e);
        }

        private void bbtn_HoSoKhamBenh_ItemClick(object sender, ItemClickEventArgs e)
        {
            XemHoSoBenhNhan xemHoSoBenhNhan = new XemHoSoBenhNhan();
            xemHoSoBenhNhan.ShowDialog();
        }

        private void btn_DangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
            BacSiKham_BenhNhan = "";
            dangNhap = new DangNhap();
            dangNhap.Show();
                
        }

        private void BacSi_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Close();
            //DangNhap dangNhap = new DangNhap();
            //dangNhap.Close();
        }
    }
}