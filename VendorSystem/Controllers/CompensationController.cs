using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Controllers
{
    public class CompensationController : MyBaseController
    {
        [AuthorizeShow(PageName = "Compensation", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CustomerUnit CustomerUnit = new CustomerUnit();
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {
                ViewBag.Customer = new SelectList(CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.StoreName + " - " + w.CompanyName + "  - " + w.CustomerCode }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Customer = new SelectList(CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.StoreNameEng + " - " + w.CompanyNameEng + "  - " + w.CustomerCode }).ToList(), "ID", "Name");
            }
            return View();
        }
        //Compensation with growth percent
        //Sameh Code
        [AuthorizeShow(PageName = "Compensation with growth percent", TypeButton = TypeButton.Show)]
        public ActionResult CompensationGrowthPercent()
        {
            CustomerUnit CustomerUnit = new CustomerUnit();
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {
                ViewBag.Customer = new SelectList(CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.StoreName + " - " + w.CompanyName + "  - " + w.CustomerCode }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Customer = new SelectList(CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.StoreNameEng + " - " + w.CompanyNameEng + "  - " + w.CustomerCode }).ToList(), "ID", "Name");
            }
            return View();
        }
        [HttpPost]
        [AuthorizeShow(PageName = "Compensation", TypeButton = TypeButton.Search)]
        public JsonResult DownloadCompensationData(int? CustomerID, string DateFrom, string DateTo)
        {

            FileVM Result = new FileVM();
            DateTime _DateFrom = new DateTime(2020, 11, 1).Date;
            DateTime _DateTo = DateTime.Now.Date;


            if (DateFrom != "")
            {
                try
                {
                    var TempDateFrom = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (TempDateFrom > _DateTo)
                    {
                        Result.Status = "Error";
                        Result.FilePath = CheckUnit.RetriveCorrectMsg("تاريخ البدايه يجب ان يكون اقل من او يساوى تاريخ اليوم", "Start date nust be less than or eual today");
                        return Json(Result);
                    }
                    if (TempDateFrom > _DateFrom)
                        _DateFrom = TempDateFrom;
                }
                catch { }
            }
            if (DateTo != "")
            {
                try
                {
                    var TempDateTo = DateTime.ParseExact(DateTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (TempDateTo < _DateFrom)
                    {
                        Result.Status = "Error";
                        Result.FilePath = CheckUnit.RetriveCorrectMsg("تاريخ النهاية يجب ان يكون اكبر  من او يساوى تاريخ البدايه", "End date nust be greated  than or eual Start Day");
                        return Json(Result);
                    }

                    if (TempDateTo < _DateTo)
                        _DateTo = TempDateTo;
                }
                catch { }
            }

            CompensationUnit CompensationUnit = new CompensationUnit();
            string Path = "";

            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            CompensationUnit.DownloadCompensationData(CustomerID, _DateFrom, _DateTo, Server, Result, Vendor_CompanyID);

            if (Result.Status == "Done")
            {
                int Count = Result.FilePath.Length - 2;
                Path = "/" + Result.FilePath.Substring(2, Count);
                Result.FilePath = Path;
            }
            return Json(Result);
        }

        //Sameh Code
        [HttpPost]
        [AuthorizeShow(PageName = "Compensation with growth percent", TypeButton = TypeButton.Search)]
        public JsonResult DownloadCompensationGrowthData(int? CustomerID, string DateFrom, string DateTo)
        {

            FileVM Result = new FileVM();
            DateTime _DateFrom = new DateTime(2020, 11, 1).Date;
            DateTime _DateTo = DateTime.Now.Date;


            if (DateFrom != "")
            {
                try
                {
                    var TempDateFrom = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (TempDateFrom > _DateTo)
                    {
                        Result.Status = "Error";
                        Result.FilePath = CheckUnit.RetriveCorrectMsg("تاريخ البدايه يجب ان يكون اقل من او يساوى تاريخ اليوم", "Start date nust be less than or eual today");
                        return Json(Result);
                    }
                    if (TempDateFrom > _DateFrom)
                        _DateFrom = TempDateFrom;
                }
                catch { }
            }
            if (DateTo != "")
            {
                try
                {
                    var TempDateTo = DateTime.ParseExact(DateTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (TempDateTo < _DateFrom)
                    {
                        Result.Status = "Error";
                        Result.FilePath = CheckUnit.RetriveCorrectMsg("تاريخ النهاية يجب ان يكون اكبر  من او يساوى تاريخ البدايه", "End date nust be greated  than or eual Start Day");
                        return Json(Result);
                    }

                    if (TempDateTo < _DateTo)
                        _DateTo = TempDateTo;
                }
                catch { }
            }

            CompensationUnit CompensationUnit = new CompensationUnit();
            string Path = "";

            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            CompensationUnit.DownloadCompensationGrowthData(CustomerID, _DateFrom, _DateTo, Server, Result, Vendor_CompanyID);

            if (Result.Status == "Done")
            {
                int Count = Result.FilePath.Length - 2;
                Path = "/" + Result.FilePath.Substring(2, Count);
                Result.FilePath = Path;
            }
            return Json(Result);
        }

    }
}