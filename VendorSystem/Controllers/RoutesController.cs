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
    public class RoutesController : MyBaseController
    {
        RouteUnit RouteUnit = new RouteUnit();


        [AuthorizeShow(PageName = "Route", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Routes = RouteUnit.GetAllRoutesVM(Vendor_CompanyID).Select(w => new RouteVM()
                {
                    Name = w.Name,
                    CountryName = w.CountryName,
                    CityName = w.CityName,
                    Note = w.Note,
                    ID = w.ID,
                    IsActive = w.IsActive,
                    RouteCode = w.RouteCode
                }).ToList();
            }
            else
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Routes = RouteUnit.GetAllRoutesVM(Vendor_CompanyID).Select(w => new RouteVM()
                {
                    Name = w.NameEng,
                    CountryName = w.CountryNameEng,
                    CityName = w.CityNameEng,
                    Note = w.Note,
                    ID = w.ID,
                    IsActive = w.IsActive,
                    RouteCode = w.RouteCode
                }).ToList();
            }
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Route", TypeButton = TypeButton.Save)]
        public JsonResult Save(RouteVM RouteVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = RouteUnit.Save(RouteVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Route", TypeButton = TypeButton.Search)]
        public JsonResult GetRouteByID(int ID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Route = RouteUnit.GetAllRoutes(Vendor_CompanyID).Where(w => w.ID == ID).Select(w => new RouteVM
            {
                CityID = w.CityID,
                CityName = "",
                CityNameEng = "",
                CountryID = w.CountryID,
                CountryName = "",
                CountryNameEng = "",
                ID = w.ID,
                IsActive = w.IsActive,
                Name = w.Name,
                NameEng = w.NameEng,
                Note = w.Note,
                ProvinceID = w.ProvinceID,
                RegionID = w.RegionID,
                TerritoryID = w.TerritoryID,
                RouteCode = w.Code
            }).FirstOrDefault();

            return Json(Route);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Route", TypeButton = TypeButton.Save)]
        public JsonResult UploadAndSave()
        {
            int? UserID = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = RouteUnit.UploadAndSave(Server, Request, UserID.Value, Vendor_CompanyID);
            return Json(Result);
        }
    }
}