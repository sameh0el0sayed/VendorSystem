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
    public class DistributorsController : MyBaseController
    {
        DistributorUnit DistributorUnit = new DistributorUnit();

        [AuthorizeShow(PageName = "Distributors", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");

                ViewBag.Distributor =  DistributorUnit.GetAllDistributorsVM(Vendor_CompanyID).Select(w => new DistributorVM()
                {
                    Address = w.Address,
                    Mobile = w.Mobile,
                    Code = w.Code,
                    Name = w.Name,
                    CityName = w.CityName,
                    IsActive = w.IsActive, 
                    RoutesCount = w.RoutesCount
                }).ToList();
            }
            else
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

                ViewBag.Distributor = DistributorUnit.GetAllDistributorsVM(Vendor_CompanyID).Select(w => new DistributorVM()
                {
                    Address = w.Address,
                    Mobile = w.Mobile,
                    Code = w.Code, 
                    Name = w.NameEng,
                    CityName = w.CityNameEng,
                    IsActive = w.IsActive,
                    RoutesCount = w.RoutesCount
                }).ToList();

            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Distributors", TypeButton = TypeButton.Save)]
        public JsonResult Save(DistributorVM DistributorVM)
        {

            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = DistributorUnit.Save(DistributorVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Distributors", TypeButton = TypeButton.Search)]
        public JsonResult GetDistributorByCode(string Code)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Rslt = DistributorUnit.GitDistriutorsByCode(Vendor_CompanyID, Code);

            return Json(Rslt);
        }
        
    }
}