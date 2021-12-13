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
    public class AllocationController : MyBaseController
    {
        AllocationUnit AllocationUnit = new AllocationUnit();

        [AuthorizeShow(PageName = "Allocation", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LockUpUnit = new LockUpUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Region = new SelectList(LockUpUnit.GetRegionByRoutes(Vendor_CompanyID)
                    .Select(w => new { ID = w.id, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Region = new SelectList(LockUpUnit.GetRegionByRoutes(Vendor_CompanyID)
                       .Select(w => new { ID = w.id, Name = w.NameEng }).ToList(), "ID", "Name");
            }
            return View();
        }


        [HttpPost]
        [AuthorizeShow(PageName = "Allocation", TypeButton = TypeButton.Search)]
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
        [AuthorizeShow(PageName = "Allocation", TypeButton = TypeButton.Search)]
        public JsonResult GetAllocationData(int RegionID, int? TerritoryID, int? RouteID, string ExpectedDeliveryDate)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            DateTime ExpectedDate;
            try { ExpectedDate = DateTime.ParseExact(ExpectedDeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            catch { ExpectedDate = DateTime.Now.AddYears(-100); }


            Session["RegionID"] = RegionID;
            Session["TerritoryID"] = TerritoryID;
            Session["RouteID"] = RouteID;
            Session["ExpectedDate"] = ExpectedDate;

            if (Lang == "ar-SA")
            {
                var Rslt = AllocationUnit.GetAllocationData(RegionID, TerritoryID, RouteID, ExpectedDate, Vendor_CompanyID).Select(w => new AllocationVM()
                {
                    Barcode = w.Barcode,
                    Name = w.Name,
                    TotalNeededQty = w.TotalNeededQty,
                    ShippedQty = w.TotalShippedQty,
                    InternalCode = w.InternalCode
                }).ToList();

                return Json(Rslt);
            }

            else
            {
                var Rslt = AllocationUnit.GetAllocationData(RegionID, TerritoryID, RouteID, ExpectedDate, Vendor_CompanyID).Select(w => new AllocationVM()
                {
                    Barcode = w.Barcode,
                    Name = w.NameEng,
                    TotalNeededQty = w.TotalNeededQty,
                    ShippedQty = w.TotalShippedQty,
                    InternalCode = w.InternalCode
                }).ToList();

                return Json(Rslt);
            }
        }


        [HttpPost]
        [AuthorizeShow(PageName = "Allocation", TypeButton = TypeButton.Save)]
        public JsonResult Save(List<AllocationVM> AllocationVMLst)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var RegionID = Session["RegionID"] as int?;  
            var TerritoryID = Session["TerritoryID"] as int?;
            var RouteID = Session["RouteID"] as int?;
            var ExpectedDate = Session["ExpectedDate"] as DateTime?;

            var Rslt = AllocationUnit.Save(RegionID.Value, TerritoryID, RouteID, ExpectedDate.Value, Vendor_CompanyID, AllocationVMLst);
            return Json(Rslt);
        }
    }
}