using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace EE.BM.Export
{
    public class ExcelExport : IDisposable
    {
        private string excel = string.Empty;

        private Excel.ApplicationClass excelApplication = null;
        private Excel.Workbook workBook = null;
        private object missing = System.Reflection.Missing.Value;

        public ExcelExport(string file)
        {
            string extension = Path.GetExtension(file).Trim('.');
            if (extension == "xls" || extension == "xlsx")
            {
                this.excel = file;

                excelApplication = new Excel.ApplicationClass()
                {
                    Visible = false,
                    DisplayAlerts = false,
                    AlertBeforeOverwriting = false
                };

                workBook = excelApplication.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            }
            else
            {
                throw new Exception("Input file is invail");
            }
            
            
        }

        public Excel._Worksheet CreateSheet(int sheetIndex, string sheetName)
        {
            var newSheet = this.workBook.Sheets.Add(this.missing, this.missing, this.missing, Excel.XlSheetType.xlWorksheet) as Excel._Worksheet;

            newSheet.Name = sheetName;

            return newSheet;
        }

        public void InsertRows(int rowIndex, int count, Excel.WorksheetClass workSheet)
        {
            try
            {
                Excel.Range range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];
                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown, missing);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess(false);
                throw e;
            }
        }

        public void CopyRows(int rowIndex, int count, Excel.WorksheetClass workSheet)
        {
            try
            {
                Excel.Range range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];
                for (int i = 1; i <= count; i++)
                {
                    Excel.Range range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess(false);
                throw e;
            }
        }

        public void InsertColumns(int columnIndex, int count, Excel.WorksheetClass workSheet)
        {
            try
            {
                Excel.Range range = (Excel.Range)(workSheet.Columns[columnIndex, this.missing]);  //注意：这里和VS的智能提示不一样，第一个参数是columnindex

                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown, missing);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess(false);
                throw e;
            }
        }

        public void DeleteColumns(int columnIndex, int count, Excel.WorksheetClass workSheet)
        {
            try
            {
                for (int i = columnIndex + count - 1; i >= columnIndex; i--)
                {
                    ((Excel.Range)workSheet.Cells[1, i]).EntireColumn.Delete(0);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess(false);
                throw e;
            }
        }

        public void MergeCells(int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text, Excel.WorksheetClass workSheet)
        {
            Excel.Range range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents(); //先把Range内容清除，合并才不会出错
            range.MergeCells = true;

            range.Value2 = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// 向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int rowIndex, int columnIndex, string text, Excel._Worksheet workSheet)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess(false);
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int rowIndex, int columnIndex, string text, string comment, Excel._Worksheet workSheet)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
                SetCellComment(rowIndex, columnIndex, comment, workSheet);
            }
            catch
            {
                this.KillExcelProcess(false);
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCellComment(int rowIndex, int columnIndex, string comment, Excel._Worksheet workSheet)
        {
            try
            {
                Excel.Range range = workSheet.Cells[rowIndex, columnIndex] as Excel.Range;
                range.AddComment(comment);
            }
            catch
            {
                this.KillExcelProcess(false);
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        // <summary>
        /// 单元格背景色及填充方式
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="color">颜色索引</param>
        public void SetCellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Range range = excelApplication.get_Range(excelApplication.Cells[startRow, startColumn], excelApplication.Cells[endRow, endColumn]);
            range.Interior.ColorIndex = color;
        }

        #region 保存文件

        public void SaveAsFile()
        {
            if (this.excel == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                this.workBook.SaveAs(excel, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Quit();
            }
        }

        /// <summary>
        /// 另存文件
        /// </summary>
        public void SaveAsFile(Excel.WorkbookClass workBook)
        {
            if (this.excel == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                workBook.SaveAs(excel, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Quit();
            }
        }

        /// <summary>
        /// 将Excel文件另存为指定格式
        /// </summary>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void SaveAsFile(SaveAsFileFormat format, Excel.WorkbookClass workBook)
        {
            if (this.excel == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                switch (format)
                {
                    case SaveAsFileFormat.HTML:
                        {
                            workBook.SaveAs(excel, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case SaveAsFileFormat.CSV:
                        {
                            workBook.SaveAs(excel, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case SaveAsFileFormat.TEXT:
                        {
                            workBook.SaveAs(excel, Excel.XlFileFormat.xlUnicodeText, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case SaveAsFileFormat.XML:
                        {
                            workBook.SaveAs(excel, Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                             Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                             Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            break;
                        }
                    default:
                        {
                            workBook.SaveAs(excel, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Quit();
            }
        }

        #endregion

        #region 杀进程释放资源

        /// <summary>
        /// 结束Excel进程
        /// </summary>
        public void KillExcelProcess(bool bAll)
        {
            if (bAll)
            {
                KillAllExcelProcess();
            }
            else
            {
                KillSpecialExcel();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        /// <summary>
        /// 杀特殊进程的Excel
        /// </summary>
        public void KillSpecialExcel()
        {
            try
            {
                if (excelApplication != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId((IntPtr)excelApplication.Hwnd, out lpdwProcessId);

                    if (lpdwProcessId > 0)    //c-s方式
                    {
                        System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                    }
                    else
                    {
                        Quit();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Quit()
        {

            if (excelApplication != null)
            {
                excelApplication.Workbooks.Close();
                excelApplication.Quit();
            }

            if (excelApplication != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplication);
                excelApplication = null;
            }
            GC.Collect();
        }

        /// <summary>
        /// 接口方法 释放资源
        /// </summary>
        public void Dispose()
        {
            Quit();
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 杀Excel进程
        /// </summary>
        public static void KillAllExcelProcess()
        {
            Process[] myProcesses;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间
            foreach (Process myProcess in myProcesses)
            {
                myProcess.Kill();
            }
        }

        /// <summary>
        /// 打开相应的excel
        /// </summary>
        /// <param name="filepath"></param>
        public static void OpenExcel(string filepath)
        {
            Excel.Application xlsApp = new Excel.Application();
            xlsApp.Workbooks.Open(filepath);
            xlsApp.Visible = true;
        }

        #endregion

    }

    /// <summary>
    /// 常用颜色定义,对就Excel中颜色名
    /// </summary>
    public enum ColorIndex
    {
        无色 = -4142,
        自动 = -4105,
        黑色 = 1,
        褐色 = 53,
        橄榄 = 52,
        深绿 = 51,
        深青 = 49,
        深蓝 = 11,
        靛蓝 = 55,
        灰色80 = 56,
        深红 = 9,
        橙色 = 46,
        深黄 = 12,
        绿色 = 10,
        青色 = 14,
        蓝色 = 5,
        蓝灰 = 47,
        灰色50 = 16,
        红色 = 3,
        浅橙色 = 45,
        酸橙色 = 43,
        海绿 = 50,
        水绿色 = 42,
        浅蓝 = 41,
        紫罗兰 = 13,
        灰色40 = 48,
        粉红 = 7,
        金色 = 44,
        黄色 = 6,
        鲜绿 = 4,
        青绿 = 8,
        天蓝 = 33,
        梅红 = 54,
        灰色25 = 15,
        玫瑰红 = 38,
        茶色 = 40,
        浅黄 = 36,
        浅绿 = 35,
        浅青绿 = 34,
        淡蓝 = 37,
        淡紫 = 39,
        白色 = 2
    }


    /// <summary>
    /// HTML，CSV，TEXT，EXCEL，XML
    /// </summary>
    public enum SaveAsFileFormat
    {
        HTML,
        CSV,
        TEXT,
        EXCEL,
        XML
    }

}
