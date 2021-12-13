using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class FileManager
    {

        public void ExportExcel<T>(string WorkSheetName, List<string[]> HeaderRow, List<T> CellsData, HttpServerUtilityBase Server, FileVM _Result)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                RemoveOldFiles(Server);
                try
                {
                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:CA1";
                    //+ Char.ConvertFromUtf32(HeaderRow[0].Length + 64) + "1";

                    // Target a worksheet
                    excel.Workbook.Worksheets.Add(WorkSheetName);
                    var worksheet = excel.Workbook.Worksheets[WorkSheetName];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(HeaderRow);
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 10;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                    //worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                    worksheet.Cells[2, 1].LoadFromCollection(CellsData);
                    DateTime dt = DateTime.Now;
                    string filename = worksheet + " " + dt.Year + "-" + dt.Month + "-" + dt.Day + "--" + dt.Hour + "-" + dt.Minute + "-" + dt.Second;
                    string path = Path.GetFullPath(Server.MapPath("~/Content/Excell/Result/" + filename + ".xlsx"));

                    FileInfo excelFile = new FileInfo(path);
                    excel.SaveAs(excelFile);
                    _Result.Status = "Done";

                    _Result.FilePath = "~/Content/Excell/Result/" + filename + ".xlsx";
                }
                catch (Exception Ex)
                {
                    _Result.Status = "Error";
                    _Result.FilePath = ErrorUnit.RetriveExceptionMsg(Ex);
                }
            }
        }

        private void RemoveOldFiles(HttpServerUtilityBase Server)
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/Content/Excell/Result"));
            DateTime dt = DateTime.Now;
            var OldDate = dt.AddHours(-1);
            string OldFilesName_ThisHour = dt.Year + "-" + dt.Month + "-" + dt.Day + "--" + dt.Hour;
            string OldFilesName_LastHour = OldDate.Year + "-" + OldDate.Month + "-" + OldDate.Day + "--" + OldDate.Hour;

            foreach (string filePath in files)
            {
                if (!(filePath.Contains(OldFilesName_ThisHour) || filePath.Contains(OldFilesName_LastHour)))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch { }
                }
            }
        }

        public string CreateNewFileFromRequist(HttpServerUtilityBase Server, HttpRequestBase Request, int fileIndex, string _FileName)
        {
            RemoveOldFiles(Server);
            HttpPostedFileBase Uploadfile = Request.Files[fileIndex];
            DateTime dt = DateTime.Now;
            string FileName = _FileName + dt.Year + "-" + dt.Month + "-" + dt.Day + "--" + dt.Hour + "-" + dt.Minute + "-" + dt.Second;
            string path = Path.GetFullPath(Server.MapPath("~/Content/Excell/Result/" + FileName + ".xlsx"));
            Uploadfile.SaveAs(path);
            return path;
        }

        public void PrepareForReadingFromSpecificFile(string Path, out ExcelWorksheet worksheet, out int rows, out int columns)
        {
            FileInfo fileInfo = new FileInfo(Path);
            ExcelPackage package = new ExcelPackage(fileInfo);
            worksheet = package.Workbook.Worksheets.FirstOrDefault();
            // get number of rows and columns in the sheet
            rows = worksheet.Dimension.Rows;
            columns = worksheet.Dimension.Columns;
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "-");
        }

    }
}