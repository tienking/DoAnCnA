﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace QuanLyPhongKham
{
    class function
    {
        public static void Notice(string noidung, int type)
        {
            switch (type)
            {
                case 0:
                    MessageBox.Show(noidung, "Lỗi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    break;
                case 1:
                    MessageBox.Show(noidung, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    break;
            }
        }
        public static Boolean checkNull(Control form)
        {
            try
            {
                foreach (Control a in form.Controls)
                {
                    if (a is TextBox)
                    {
                        TextBox textBox = (TextBox)a;
                        if (textBox.Text == "")
                        {
                            Notice("Bạn phải nhập vào ô nhập còn trống", 0);                            
                            return false;
                        }         
                    }
                    
                    if (a is ComboBox)
                    {
                        ComboBox comboBox = (ComboBox)a;
                        if (comboBox.Text=="")
                        {
                            Notice("Bạn phải nhập vào ô lựa chọn còn trống", 0); return false;
                        }
                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public static string toMD5(string matkhau)
        {
            MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
            byte[] myPass = System.Text.Encoding.UTF8.GetBytes(matkhau);
            myPass = myMD5.ComputeHash(myPass);

            StringBuilder s = new StringBuilder();
            foreach (byte p in myPass)
            {
                s.Append(p.ToString("x").ToLower());
            }
            return s.ToString();
        }
        public static void ClearControl(Control form)//hàm clear tất cả
        {
            //if (form is TabPage)
            //{
            //    TabPage tab = (TabPage)form;
            //    tab.Refresh();
            //}
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBox.Text = "";
                }

                if (control is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)control;
                    if (comboBox.Items.Count > 0)
                    {
                        comboBox.SelectedIndex = 0;
                        comboBox.Text = "";
                        comboBox.Items.Clear();
                    }
                }

                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }
                if (control is DateTimePicker)
                {
                    DateTimePicker dtp = (DateTimePicker)control;
                    dtp.Value = DateTime.Now;
                }
                if (control is RadioButton)
                {
                    RadioButton rdbtn = (RadioButton)control;
                    rdbtn.Checked = false;
                }
                if (control is ListBox)
                {
                    ListBox listBox = (ListBox)control;
                    listBox.ClearSelected();
                }
                if (control is ListView)
                {
                    ListView listview = (ListView)control;
                    listview.Items.Clear();
                }
                if(control is PictureBox)
                {
                    PictureBox pictureBox = (PictureBox)control;
                    pictureBox.Image=null;
                }
            }
        }

        //vẽ cột số thứ tự cho gridview
        bool indicatorIcon = true;
        public void CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }

        public void KoNhapKiTu(object sender,KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        public void ToExcel(string noidung,DialogResult result,GridControl gridControl)
        {
            if (MessageBox.Show(noidung, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = "D:";
                save.Filter = "Select File |*.xlsx||*.docx";
                
                result = save.ShowDialog();
                string link = save.FileName;
                save.RestoreDirectory = true;
                if (result == DialogResult.OK)
                {
                    if (save.FilterIndex == 1)
                        gridControl.ExportToXlsx(link);
                    if (save.FilterIndex == 2)
                        gridControl.ExportToDocx(link);
                }
            }
        }

        //public static object OpenFile(string fileName)
        //{
        //    var fullFileName = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), fileName);
        //    if (!File.Exists(fileName))
        //    {
        //        System.Windows.Forms.MessageBox.Show("File not found");
        //        return null;
        //    }
        //    var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
        //    var adapter = new OleDbDataAdapter("select * from [Sheet1$]", connectionString);
        //    var ds = new DataSet();
        //    string tableName = ;
        //    adapter.Fill(ds, tableName);
        //    DataTable data = ds.Tables[tableName];
        //    return data;
        //}


    }
}
