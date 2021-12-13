using VendorSystem.Authorize;
using VendorSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using VendorSystem.ViewModel;
using VendorSystem.Repository;
using System.Threading;

namespace VendorSystem.Controllers
{
    public class ReplenishmentReportController : MyBaseController
    {

        [AuthorizeShow(PageName = "Replenishment Report", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CustomerUnit CustomerUnit = new CustomerUnit();
            RouteUnit RouteUnit = new RouteUnit();
            LockUpUnit LockUpUnit = new LockUpUnit();

            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            if (Lang == "ar-SA")
            {
                ViewBag.Region = new SelectList(LockUpUnit.GetRegionByRoutes(Vendor_CompanyID).Select(w => new { ID = w.id, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Region = new SelectList(LockUpUnit.GetRegionByRoutes(Vendor_CompanyID).Select(w => new { ID = w.id, Name = w.NameEng }).ToList(), "ID", "Name");
            }
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Replenishment Report", TypeButton = TypeButton.Search)]
        public JsonResult GetRouteByTerritoryID(int TerritoryID)

        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {
                var Rslt = new RouteUnit().GetRouteByTerritoryID(TerritoryID, Vendor_CompanyID).Select(w => new ID_NameVM()
                {
                    ID = w.ID,
                    Name = w.Name
                }).ToList();

                return Json(Rslt);
            }

            else
            {
                var Rslt = new RouteUnit().GetRouteByTerritoryID(TerritoryID, Vendor_CompanyID).Select(w => new ID_NameVM()
                {
                    ID = w.ID,
                    Name = w.NameEng
                }).ToList();

                return Json(Rslt);
            }
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Replenishment Report", TypeButton = TypeButton.Search)]
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
        [AuthorizeShow(PageName = "Replenishment Report", TypeButton = TypeButton.Search)]
        public JsonResult Download_Excel(string DateFrom, string DateTO, int? RegionID, int? TerritoryID, int? RouteID, int? CustDtlID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

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

            string Path = "";
            FileVM Result = new FileVM();

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            if (Lang == "ar-SA")
            {
                new ReplenishmentReportUnit().Download_Excel(Server, Result, DF, DT, RegionID, TerritoryID, RouteID, CustDtlID, Vendor_CompanyID, true);
            }
            else
            {
                new ReplenishmentReportUnit().Download_Excel(Server, Result, DF, DT, RegionID, TerritoryID, RouteID, CustDtlID, Vendor_CompanyID, false);
            }
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