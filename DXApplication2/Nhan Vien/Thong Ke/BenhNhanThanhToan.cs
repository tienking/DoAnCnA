﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Data.SqlClient;
namespace QuanLyPhongKham
{
    public partial class BenhNhanThanhToan : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        connection connection = new connection();
        function function = new function();
        SqlDataAdapter sqlDataAdapter;
        BindingSource bindingSource = new BindingSource();
        reportDonThuoc reportDonThuoc = new reportDonThuoc();
        DataSet dataSet = new DataSet();
        DangNhap dangNhap = new DangNhap();
        DialogResult result;
        string ngay = DateTime.Now.Day.ToString("d2");
        string thang = DateTime.Now.Month.ToString("d2");
        string nam = DateTime.Now.Year.ToString();
        public BenhNhanThanhToan()
        {
            InitializeComponent();
            Load_HoaDon();
        }

        private void Load_HoaDon()
        {
            string query = @"select BN.MaSoBenhNhan,BN.Ten, BN.Ho, BN.SoDienThoai,BN.DiaChi,BN.NamSinh,HSKB.MaSoKhamBenh," +
                                    " DT.MaSoDonThuoc,DT.GhiChu,HSKB.XetNghiem,HSKB.ChuanDoan,HSKB.KetQuaXetNghiem," +
                                    " HSKB.GhiChu,NV.TenNhanVien,HD.MaHoaDon,HSKB.NgayGioKham,DT.TongTienThuoc," +
                                    " HSKB.TienKham,HD.TongTien,HSKB.NgayTaiKham,HD.KiemTraThanhToan,HD.KiemTraLayThuoc" +
                            " from HoaDon HD join HoSoKhamBenh HSKB on HD.MaSoKhamBenh = HSKB.MaSoKhamBenh " +
                            " join BenhNhan BN on HSKB.MaSoBenhNhan = BN.MaSoBenhNhan" +
                            " join NhanVien NV on HSKB.MaSoBacSi = NV.MaSoNhanVien" +
                            " join DonThuoc DT on DT.MaSoDonThuoc = HD.MaSoDonThuoc" +
                            " where (HD.NgayGioLap like '" + ngay + "/" + thang + "/" + nam + "' And HD.KiemTraThanhToan = 1) or (HD.NgayGioLap like '" + ngay + "/" + thang + "/" + nam + "' And HD.KiemTraLayThuoc = 1)";
            connection.connect();
            sqlDataAdapter = new SqlDataAdapter(query, connection.con);
            dataSet = new DataSet();
            dataSet.Clear();
            sqlDataAdapter.Fill(dataSet, "HoaDon");

            bindingSource.DataSource = dataSet.Tables["HoaDon"];
            gridControl1_HoaDon.DataSource = bindingSource;
            connection.disconnect();
        }

        private void barButtonItem1_XuatFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl1_HoaDon.Print();
        }

        private void barButtonItem1_ToExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            function.ToExcel("Bạn có muốn Xuất Danh Sách ra File Excel??",result,gridControl1_HoaDon);
        }
    }
}