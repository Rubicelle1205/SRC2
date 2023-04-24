using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net.Http;
using NPOI.SS.Formula.Functions;

namespace Utility
{
    public enum ExcelVersion
    {
        Excel2003,
        Excel2007
    }

    public static class ExcelUtil
    {
        #region 相關變數
        //字串型態的日期：民國年/月/日
        public static string pattern_ChnDate = "^[0-9]{1,3}/(((0{0,1}[13578]|(10|12))/(0{0,1}[1-9]|[1-2][0-9]|3[0-1]))|(0{0,1}2/(0{0,1}[1-9]|[1-2][0-9]))|((0{0,1}[469]|11)/(0{0,1}[1-9]|[1-2][0-9]|30)))$";
        #endregion

        #region 上傳
        /// <summary> 讀取Excel </summary>
        public static DataTable LoadExcelData(Stream ExcelFile, ExcelVersion version, string LoginId)
        {
            if (ExcelFile.Length == 0)
            { return null; }

            try
            {
                DataSet ds = ExcelUtil.ImportExcel(ExcelFile, version);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                { return null; }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion 上傳

        #region 匯出

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="SheetName">設定Sheet名稱</param>
        /// <param name="LstColumnWidth">設定欄位寬度</param>
        /// <returns></returns>
        public static ISheet GenNewSheet(IWorkbook workbook, string SheetName, List<int> LstColumnWidth = null)
        {
            ISheet sheet = workbook.CreateSheet(SheetName);

            //欄位寬度設定
            if (LstColumnWidth != null)
            {
                for (int i = 0; i <= LstColumnWidth.Count - 1; i++)
                {
                    sheet.SetColumnWidth(i, LstColumnWidth[i] * 256);
                }
            }

            return sheet;

        }

        /// <summary>
        /// 設定標題Style
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static XSSFCellStyle GetDefaultHeaderStyle(IWorkbook workbook)
        {
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.IsBold = true;
            font.FontName = "微軟正黑體";
            font.FontHeightInPoints = 12;

            XSSFCellStyle Style = (XSSFCellStyle)workbook.CreateCellStyle();
            Style.SetFont(font);
            Style.Alignment = HorizontalAlignment.Center;
            Style.BorderTop = BorderStyle.Thin;
            Style.BorderBottom = BorderStyle.Thin;
            Style.BorderLeft = BorderStyle.Thin;
            Style.BorderRight = BorderStyle.Thin;

            return Style;
        }

        /// <summary>
        /// 設定內容Style
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static XSSFCellStyle GetDefaultContentStyle(IWorkbook workbook)
        {
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontName = "微軟正黑體";
            font.FontHeightInPoints = 12;

            XSSFCellStyle Style = (XSSFCellStyle)workbook.CreateCellStyle();
            Style.SetFont(font);
            Style.Alignment = HorizontalAlignment.Left;
            Style.BorderTop = BorderStyle.Thin;
            Style.BorderBottom = BorderStyle.Thin;
            Style.BorderLeft = BorderStyle.Thin;
            Style.BorderRight = BorderStyle.Thin;

            return Style;
        }


        /// <summary> 取得 DataTable 寫入後的 Excel </summary>
        public static IWorkbook RenderToExcelWorkBook(DataSet ds, ExcelVersion version)
        {
            return RenderToExcelWorkBook(0, 0, ds, version);
        }

        /// <summary> 取得 DataTable 寫入後的 Excel (StartRow:起始列, StartCol:起始欄, 起始值0)  </summary>
        public static IWorkbook RenderToExcelWorkBook(int StartRow, int StartCol, DataSet ds, ExcelVersion version)
        {
            // 產生 Excel 資料流。 
            MemoryStream ResultMs = RenderDataTableToExcel(StartRow, StartCol, ds, version) as MemoryStream;
            IWorkbook workbook;
            MemoryStream ms = new MemoryStream(ResultMs.ToArray());
            if (version == ExcelVersion.Excel2003)
            { workbook = new HSSFWorkbook(ms); }
            else
            { workbook = new XSSFWorkbook(ms); }

            return workbook;
        }

        public static void RenderDataTableToExcel(DataSet ds, string FileName, ExcelVersion version)
        {
            MemoryStream ms = RenderDataTableToExcel(ds, version) as MemoryStream;
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }

        private static Stream RenderDataTableToExcel(DataSet ds, ExcelVersion version)
        {
            IWorkbook workbook;

            if (version == ExcelVersion.Excel2003)
            { workbook = new HSSFWorkbook(); }
            else
            { workbook = new XSSFWorkbook(); }

            MemoryStream ms = new MemoryStream();
            ISheet sheet = null;
            IRow headerRow = null;

            foreach (DataTable tb in ds.Tables)
            {
                if (!string.IsNullOrEmpty(tb.TableName))
                    sheet = workbook.CreateSheet(tb.TableName);
                else
                    sheet = workbook.CreateSheet();

                headerRow = sheet.CreateRow(0);

                // handling header. 
                foreach (DataColumn column in tb.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                // handling value. 
                int rowIndex = 1;

                foreach (DataRow row in tb.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in tb.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }


            workbook.Write(ms, true);
            ms.Flush();


            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        private static MemoryStream RenderDataTableToExcel(int StartRow, int StartCol, DataSet ds, ExcelVersion version)
        {
            IWorkbook workbook;

            if (version == ExcelVersion.Excel2003)
            { workbook = new HSSFWorkbook(); }
            else
            { workbook = new XSSFWorkbook(); }

            if (StartRow < 0)
            { StartRow = 0; }

            if (StartCol < 0)
            { StartCol = 0; }

            MemoryStream ms = new MemoryStream();
            ISheet sheet = null;
            IRow headerRow = null;

            foreach (DataTable tb in ds.Tables)
            {
                int TotalCol = tb.Columns.Count + StartCol;
                int TotalRow = tb.Rows.Count + StartRow;

                if (!string.IsNullOrEmpty(tb.TableName))
                    sheet = workbook.CreateSheet(tb.TableName);
                else
                    sheet = workbook.CreateSheet();

                // if start row not 0 then create empty row
                for (int row = 0; row < StartRow; row++)
                {
                    sheet.CreateRow(row);
                    for (int col = 0; col < TotalCol; col++)
                    {
                        sheet.GetRow(row).CreateCell(col);
                    }
                }

                headerRow = sheet.CreateRow(StartRow);

                // if headerRow start col not 0 then create empty col
                for (int col = 0; col < StartCol; col++)
                {
                    headerRow.CreateCell(col);
                }

                // handling header. 
                for (int col = 0; col < TotalCol; col++)
                {
                    if (col < StartCol)
                    {
                        headerRow.CreateCell(col);
                        continue;
                    }
                    int Index = col - StartCol;
                    headerRow.CreateCell(col).SetCellValue(tb.Columns[Index].ColumnName);
                }

                // handling value. 
                int rowIndex = StartRow + 1;

                foreach (DataRow row in tb.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    // if col start col not 0 then create empty col
                    for (int col = 0; col < StartCol; col++)
                    {
                        dataRow.CreateCell(col);
                    }

                    foreach (DataColumn column in tb.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal + StartCol).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }


            workbook.Write(ms, true);
            ms.Flush();

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        #endregion

        #region 匯入

        /// <summary> 匯入Excel(可匯入多sheet)，標題列不可空白!! </summary>
        public static DataSet ImportExcel(Stream stream, ExcelVersion version)
        {
            stream.Position = 0;
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            if (version == ExcelVersion.Excel2003)
                return XLS_Reader(ms);
            else
                return XLSX_Reader(ms);
        }

        /// <summary>
        /// Excel97-2003
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static DataSet XLS_Reader(Stream stream)
        {
            DataSet ds = new DataSet();

            stream.Position = 0;
            HSSFWorkbook wb = new HSSFWorkbook(stream);

            for (int k = 0; k < wb.NumberOfSheets; k++)
            {
                HSSFSheet sheet = (HSSFSheet)wb.GetSheetAt(k);
                DataTable table = new DataTable();
                //由第一列取標題做為欄位名稱
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                if (headerRow == null) continue;    //遇到空sheet就跳過
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null)
                    {
                        cellCount = i;  //標題欄遇到空白就結束，順便重設總欄數
                        break;
                    }
                    //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                    table.Columns.Add(new DataColumn(headerRow.GetCell(i).StringCellValue));
                }

                //略過第零列(標題列)，一直處理至最後一列
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    if (row == null) continue;
                    DataRow dataRow = table.NewRow();
                    //依先前取得的欄位數逐一設定欄位內容
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) == null)     //下拉選單的格式是BLANK，但完全未選未動時是null
                            dataRow[j] = string.Empty;
                        else if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            dataRow[j] = row.GetCell(j).StringCellValue;
                        }
                        else if (row.GetCell(j).CellType == CellType.Numeric)    //日期需另外處理
                        {
                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))       //西曆，西元年/月/日，DataFormat=14
                                dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            else if (row.GetCell(j).CellStyle.DataFormat == 176)    //中國民國曆，民國年/月/日
                                dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            else if (row.GetCell(j).CellStyle.DataFormat == 177)//中國民國曆，民國年/月/日
                            {
                                //沒辦法有人就不用範本檔，導致格式會錯誤，所以只能先這樣擋一擋
                                if (int.Parse(row.GetCell(j).DateCellValue.ToString("yyyy")) <= 1900)
                                    dataRow[j] = row.GetCell(j).ToString();
                                else
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            }
                            else
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        else
                        {
                            if (Regex.IsMatch(row.GetCell(j).ToString(), pattern_ChnDate))      //字串型式的民國年，改為西元年輸出
                            {
                                string[] tmp = row.GetCell(j).ToString().Split('/');
                                dataRow[j] = Convert.ToString(int.Parse(tmp[0]) + 1911) + '/' + tmp[1] + '/' + tmp[2];
                            }
                            else
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    table.Rows.Add(dataRow);
                }

                ds.Tables.Add(table);
            }
            return ds;
        }

        /// <summary>
        /// Excel2007
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static DataSet XLSX_Reader(Stream stream)
        {
            DataSet ds = new DataSet();

            stream.Position = 0;
            XSSFWorkbook wb = new XSSFWorkbook(stream);

            for (int k = 0; k < wb.NumberOfSheets; k++)
            {
                XSSFSheet sheet = (XSSFSheet)wb.GetSheetAt(k);
                DataTable table = new DataTable();
                //由第一列取標題做為欄位名稱
                XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);
                if (headerRow == null) continue;    //遇到空sheet就跳過
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null)
                    {
                        cellCount = i;  //標題欄遇到空白就結束，順便重設總欄數
                        break;
                    }
                    //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                    table.Columns.Add(new DataColumn(headerRow.GetCell(i).StringCellValue));
                }

                //略過第零列(標題列)，一直處理至最後一列
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    XSSFRow row = (XSSFRow)sheet.GetRow(i);
                    if (row == null) continue;
                    DataRow dataRow = table.NewRow();
                    //依先前取得的欄位數逐一設定欄位內容
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) == null)     //下拉選單的格式是BLANK，但完全未選未動時是null
                            dataRow[j] = string.Empty;
                        else if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            row.GetCell(j).SetCellType(CellType.String);
                            dataRow[j] = row.GetCell(j).StringCellValue;
                        }
                        else if (row.GetCell(j).CellType == CellType.Numeric)    //日期需另外處理
                        {
                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))       //西曆，西元年/月/日，DataFormat=14
                                dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            else if (row.GetCell(j).CellStyle.DataFormat == 176)    //中國民國曆，民國年/月/日
                                dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            else if (row.GetCell(j).CellStyle.DataFormat == 177)    //中國民國曆，民國年/月/日
                                dataRow[j] = row.GetCell(j).DateCellValue.ToString("yyyy/MM/dd");
                            else
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        else
                        {
                            if (Regex.IsMatch(row.GetCell(j).ToString(), pattern_ChnDate))      //字串型式的民國年，改為西元年輸出
                            {
                                string[] tmp = row.GetCell(j).ToString().Split('/');
                                dataRow[j] = Convert.ToString(int.Parse(tmp[0]) + 1911) + '/' + tmp[1] + '/' + tmp[2];
                            }
                            else
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    table.Rows.Add(dataRow);
                }

                ds.Tables.Add(table);
            }

            return ds;
        }


        #endregion

    }
}
