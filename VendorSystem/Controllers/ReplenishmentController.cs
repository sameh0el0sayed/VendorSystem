using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.Models.Model1;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Controllers
{
    public class ReplenishmentController : MyBaseController
    {
        BayanEntities _context = new BayanEntities();

        [AuthorizeShow(PageName = "Replenishment", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CustomerUnit CustomerUnit = new CustomerUnit();
            RouteUnit RouteUnit = new RouteUnit();
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            if (Lang == "ar-SA")
            {
                ViewBag.Customers = new SelectList(CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { Code = w.ID, Name = w.CompanyName + " - " + w.StoreName + " - " + w.CustomerCode }).ToList(), "Code", "Name");
                ViewBag.Route = new SelectList(RouteUnit.GetAllActiveRoutes(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name + " - " + w.Code }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Customers = new SelectList(CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { Code = w.ID, Name = w.CompanyNameEng + " - " + w.StoreNameEng + " - " + w.CustomerCode }).ToList(), "Code", "Name");
                ViewBag.Route = new SelectList(RouteUnit.GetAllActiveRoutes(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng + " - " + w.Code }).ToList(), "ID", "Name");
            }
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Replenishment", TypeButton = TypeButton.Search)]
        public JsonResult GetCustomerByRouteID(int RouteID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var CustomerUnit = new CustomerUnit();
            var Data = CustomerUnit.GetCustomersVM_ByRouteID(RouteID, Vendor_CompanyID);
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            if (Lang == "ar-SA")
            {
                var Rslt = Data.Select(w => new { ID = w.ID, Name = w.CompanyName + " - " + w.StoreName + " -- " + w.CustomerCode }).ToList();
                return Json(Rslt);
            }
            else
            {
                var Rslt = Data.Select(w => new { ID = w.ID, Name = w.CompanyNameEng + " - " + w.StoreNameEng + " -- " + w.CustomerCode }).ToList();
                return Json(Rslt);
            }
        }


        [HttpPost]
        public JsonResult CompareDates(string DateFrom, string DateTO)
        {
            #region Two Dates Have values
            DateTime? Start = null;
            DateTime? End = null;
            try { Start = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            catch { Start = null; }
            try { End = DateTime.ParseExact(DateTO, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            catch { End = null; }

            DateTime TodayDate = DateTime.Now;
            if (Start != null && Start > TodayDate)
            {
                return Json(3, JsonRequestBehavior.AllowGet);
            }
            else if (End != null && End > TodayDate)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
            else if ((Start != null && End != null) && Start > End)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(4, JsonRequestBehavior.AllowGet);

            #endregion

        }


        [HttpPost]
        [AuthorizeShow(PageName = "Replenishment", TypeButton = TypeButton.Search)]
        public JsonResult GetReplenishData(string DateFrom, string DateTO, int? RouteID, int? CustDtlID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            Session["Replenish_Data"] = null;
            DateTime? DF = null;
            DateTime? DT = null;
            try { DF = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            catch { DF = null; }
            try { DT = DateTime.ParseExact(DateTO, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            catch { DT = null; }

            if (DF != null)
                DF = DF.Value.AddDays(-1);
            if (DT != null)
                DT = DT.Value.AddDays(1);
            //Sameh Edit
            Session["Replenish_Data"] = null;
            Session["Replenish_Data"] = _context.V_Replenish_Results.Where(w => (CustDtlID == null || w.CustDtlID == CustDtlID.Value) && w.Vendor_CompanyID == Vendor_CompanyID
                                                                     && (w.DateFrom > DF) && (w.DateTo < DT) && (RouteID == null || w.RouteID == RouteID))
                                          .Select(w => new Replenish_VM()
                                          {
                                              Avg_Po_Return = w.Avg_Po_Return,
                                              Barcode = w.Barcode,
                                              CompanyName = w.CompanyName,
                                              Current_Stock = w.Current_Stock,
                                              DateFrom = w.DateFrom,
                                              DateTo = w.DateTo,
                                              First_Visit = w.First_Visit,
                                              Forecasted_Po_Qty = w.Forecasted_Po_Qty,
                                              StoreName = w.Name,
                                              Net_Sales = w.Net_Sales,
                                              Product_Name = w.Product_Name,
                                              Replenish_Master_Id = w.Replenish_Master_Id,
                                              Second_Visit = w.Second_Visit,
                                              StoreID = w.StoreID,
                                              Third_Visit = w.Third_Visit,
                                              Total_Po_Qty = w.Total_Po_Qty,
                                              Total_Po_Return_Qty = w.Total_Po_Return_Qty,
                                              CustoemrCode = w.CustoemrCode,
                                              RoutCode = w.RoutCode,
                                              RouteName = w.RouteName,
                                              FirstPO_Received = w.FirstPO_Received,
                                              FirstPO_Return = w.FirstPO_Return,
                                              FifthPO_Received = w.FifthPO_Received,
                                              FifthPO_Return = w.FifthPO_Return,
                                              FourthPO_Received = w.FourthPO_Received,
                                              FourthPO_Return = w.FourthPO_Return,
                                              SecondPO_Received = w.SecondPO_Received,
                                              SecondPO_Return = w.SecondPO_Return,
                                              SixthPO_Received = w.SixthPO_Received,
                                              SixthPO_Return = w.SixthPO_Return,
                                              ThirdPO_Received = w.ThirdPO_Received,
                                              ThirdPO_Return = w.ThirdPO_Return
                                          })
                                         .ToList();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AuthorizeShow(PageName = "Replenishment", TypeButton = TypeButton.Search)]
        //public JsonResult DrowDataTableRows()
        //{
        //    List<Replenish_VM> Replenish_Data = Session["Replenish_Data"] as List<Replenish_VM>;
        //    return Json(Replenish_Data);
        //}

        [HttpPost]
        [AuthorizeShow(PageName = "Replenishment", TypeButton = TypeButton.Search)]
        public JsonResult Download_Excel()
        {
            string Path = "";
            FileVM Result = new FileVM();
            string FilePath;

            Result.Status = Download_Excel(out FilePath, Server, "Replenishment_Results");
            if (FilePath != "")
            {
                int Count = FilePath.Length - 2;
                Path = "/" + FilePath.Substring(2, Count);
            }
            Result.FilePath = Path;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        private string Download_Excel(out string FilePath, HttpServerUtilityBase Server, string FileName)
        {
            string Msg = "Done";
            try
            {

                var headerRow = new List<string[]>() { new string[] { "Replenish_Master_Id","DateFrom", "DateTo", "Barcode", "Product_Name", "Sub_WH_Id", "Sub_WH_Na","CompanyName",
                    " Customer ID", "Route Code", "Route Name","Current_Stock","Net_Sales", "Total_Po_Qty", "Total_Po_Return_Qty","FirstPO_Received", "FirstPO_Return", "SecondPO_Received", "SecondPO_Return",
                    "ThirdPO_Received","ThirdPO_Return", "FourthPO_Received", "FourthPO_Return", "FifthPO_Received", "FifthPO_Return", "SixthPO_Received", "SixthPO_Return",
                    "Avg_Po_Return","Forecasted_Po_Qty","First_Visit","Second_Visit","Third_Visit"} };

                var Rep_File = Session["Replenish_Data"] as List<Replenish_VM>;
                FilePath = ExportExcel(FileName, headerRow, Rep_File, Server, "");
            }
            catch (Exception ex)
            {
                FilePath = "";
                Msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return Msg;
        }

        private string ExportExcel<T>(string WorkSheetName, List<string[]> HeaderRow, List<T> CellsData, HttpServerUtilityBase Server, string EmptyString)
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

                    return "~/Content/Excell/Result/" + filename + ".xlsx";
                }
                catch
                {
                    return EmptyString;
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
    }
}