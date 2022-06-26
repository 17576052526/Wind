using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public class ExcelHelper
    {
        #region Excel 与数据库的相互导入和导出
        /// <summary>
        /// Excel转 DataTable
        /// </summary>
        /// <param name="excelStream">Excel 文件流，.netCore 调用时传参 Request.Form.Files[0].OpenReadStream()</param>
        public static DataTable ExcelToDataTable(Stream excelStream, string sheetName)
        {
            DataTable dt = new DataTable();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//设置非商业证书，不设置会报错
            using (ExcelPackage package = new ExcelPackage(excelStream))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[sheetName];//工作表
                //DataTable 添加列
                int startRow = sheet.Dimension.Start.Row;
                for (int i = sheet.Dimension.Start.Column, c = sheet.Dimension.End.Column; i <= c; i++)
                {
                    dt.Columns.Add(Convert.ToString(sheet.Cells[startRow, i].Value));
                }
                //DataTable 添加行
                for (int i = sheet.Dimension.Start.Row + 1, r = sheet.Dimension.End.Row; i <= r; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = sheet.Dimension.Start.Column, c = sheet.Dimension.End.Column; j <= c; j++)
                    {
                        dr[j - 1] = sheet.Cells[i, j].Value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }



        /// <summary>
        /// DataTable 转Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelByte">在当前已有的Excel中加一个Sheet，不传表示创建一个新的Excel</param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static byte[] DataTableToExcel(DataTable dt, byte[] excelByte = null, string sheetName = "Sheet1")
        {
            using (MemoryStream stream = excelByte == null ? new MemoryStream() : new MemoryStream(excelByte))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//设置非商业证书，不设置会报错
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add(sheetName);

                    int colCount = dt.Columns.Count;
                    //遍历列
                    for (int i = 0; i < colCount; i++)
                    {
                        sheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
                    }
                    //遍历行
                    for (int i = 0, r = dt.Rows.Count; i < r; i++)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            sheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                    //流保存到新的对象中，并转换成字节
                    using (MemoryStream newStream = new MemoryStream())
                    {
                        package.SaveAs(newStream);
                        return newStream.ToArray();
                    }
                }
            }

        }

        
        #endregion
        /*
        #region C#操作 Excel（针对已有的Excel模板做操作）,如果只是简单的Excel数据导入导出，请调用上面的方法
        
        // 使用说明：
        // 1.先调用Open()
        // 2.其他操作
        // 3.调用 SaveAs()保存文件，并释放资源
         
        private Application app;
        private _Workbook wbk;
        private _Worksheet sheet; //当前操作的sheet
        private object missing = System.Reflection.Missing.Value;
        public int RowCount { get { return sheet.UsedRange.Rows.Count; } }  //总行数
        public int ColCount { get { return sheet.UsedRange.Columns.Count; } }  //总列数
        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="path">Excel的路径</param>
        public void Open(string path, string sheetName)
        {
            app = new Application();
            Workbooks wbks = app.Workbooks;
            app.DisplayAlerts = false;  //设置是否显示警告窗体
            app.Visible = false;    //设置是否显示Excel
            app.ScreenUpdating = false; //禁止刷新屏幕
            wbk = wbks.Add(path);  //打开已有的Excel
            Sheets sheets = wbk.Sheets;  //获取sheet集合
            sheet = (_Worksheet)sheets.get_Item(sheetName); //当前操作的sheet
        }

        /// <summary>
        /// 插入行，index:索引，从1开始
        /// </summary>
        public void AddRow(int index)
        {
            ((Range)sheet.Rows[index, missing]).Insert(missing, XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        }
        /// <summary>
        /// 插入列，index:索引，从1开始
        /// </summary>
        public void AddCol(int index)
        {
            Range xlsColumns = (Range)sheet.Columns[index, System.Type.Missing];
            xlsColumns.Insert(XlInsertShiftDirection.xlShiftToRight, Type.Missing);
        }
        public void RowHeight(float height, int row1, int row2)
        {
            for (int i = row1; i <= row2; i++)
            {
                Range range = (Range)sheet.Rows[i, missing];
                range.RowHeight = height;
            }

        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        public void SetCell(int row, int cell, string val)
        {
            Range r = (Range)sheet.Cells[row, cell];
            r.Value = val;
            r.Borders.LineStyle = 1;
        }
        /// <summary>
        /// 获取单元格的值
        /// </summary>
        public string GetCell(int row, int cell)
        {
            return Convert.ToString(sheet.Cells[row, cell].Value);
        }
        //该方法注释掉 因为要引用 COM：Microsoft Office 14.0 Object Library
        /// <summary>
        /// 向Excel中插入图片  RangeName：单元格名称，例如：B4 ,其他参数分别为图片的路径,图片宽度和高度
        /// </summary>
        //public void InsertImg(string RangeName, string imgPath, float width, float height)
        //{
        //    Range range = sheet.get_Range(RangeName, missing);
        //    float left = Convert.ToSingle(range.Left);
        //    float top = Convert.ToSingle(range.Top);
        //    sheet.Shapes.AddPicture(imgPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);
        //}
        /// <summary>
        /// 保存文件并释放资源
        /// </summary>
        /// <param name="path"></param>
        public void SaveAs(string path)
        {
            //屏蔽掉系统跳出的Alert
            app.AlertBeforeOverwriting = false;
            //保存到指定目录
            wbk.SaveAs(path, missing, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            //释放资源
            wbk.Close();
            app.Quit();
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("excel");
            foreach (System.Diagnostics.Process pro in procs) { pro.Kill(); }  //杀死进程
        }
        #endregion
        */
    }
}
